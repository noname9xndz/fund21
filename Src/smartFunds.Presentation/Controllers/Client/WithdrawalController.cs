using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using smartFunds.Common.Helpers;
using smartFunds.Infrastructure.Models;
using smartFunds.Infrastructure.Services;
using smartFunds.Model.Common;
using smartFunds.Model.Resources;
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Controllers.Client
{
    [Route("withdrawal")]
    public class WithdrawalController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFundTransactionHistoryService _fundTransactionHistoryService;
        private readonly IInvestmentTargetService _investmentTargetService;
        private readonly IConfiguration _configuration;
        private readonly ISMSGateway _smsGateway;
        private readonly IWithdrawFeeService _withdrawFeeService;
        private readonly IViettelPay _viettelPay;
        private readonly IOrderRequestService _orderRequestService;

        public WithdrawalController(IUserService userService, IFundTransactionHistoryService fundTransactionHistoryService, IInvestmentTargetService investmentTargetService, IConfiguration configuration, ISMSGateway smsGateway, IWithdrawFeeService withdrawFeeService, IViettelPay viettelPay, IOrderRequestService orderRequestService)
        {
            _userService = userService;
            _fundTransactionHistoryService = fundTransactionHistoryService;
            _configuration = configuration;
            _investmentTargetService = investmentTargetService;
            _smsGateway = smsGateway;
            _withdrawFeeService = withdrawFeeService;
            _viettelPay = viettelPay;
            _orderRequestService = orderRequestService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Withdrawal()
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await _userService.GetCurrentUser();

            var newOrder = new OrderRequestModel()
            {
                PhoneNumber = "84" + currentUser.PhoneNumber.Remove(0, 1),
                FullName = currentUser.FullName
            };
            var order = await _orderRequestService.SaveOrder(newOrder);

            var viettelPayApi = _configuration.GetValue<bool>("RequestPaymentLink:IsLive") ? _configuration.GetValue<string>("RequestPaymentLink:Live") : _configuration.GetValue<string>("RequestPaymentLink:Test");
            var cmd = _configuration.GetValue<string>("RequestPaymentParam:cmdCheckAccount");
            var rsaPublicKey = _configuration.GetValue<string>("RSAKey:public");
            var rsaPrivateKey = _configuration.GetValue<string>("RSAKey:private");
            var rsaPublicKeyVTP = _configuration.GetValue<string>("RSAKey:VTPpublic");

            var rsa = new RSAHelper(RSAType.RSA, Encoding.UTF8, "", rsaPublicKeyVTP);
            var passwordEncrypt = rsa.Encrypt(_configuration.GetValue<string>("RequestPaymentParam:password"));

            var dataCheckAccount = new DataCheckAccount()
            {
                msisdn = "84" + currentUser.PhoneNumber.Remove(0, 1),
                customerName = currentUser.FullName
            };

            var soapDataCheckAccount = new SoapDataCheckAccount()
            {
                username = _configuration.GetValue<string>("RequestPaymentParam:username"),
                password = passwordEncrypt,
                serviceCode = _configuration.GetValue<string>("RequestPaymentParam:serviceCode"),
                orderId = order.Id.ToString()
            };

            var code = _viettelPay.CheckAccount(viettelPayApi, cmd, rsaPublicKey, rsaPrivateKey, rsaPublicKeyVTP, dataCheckAccount, soapDataCheckAccount);

            if (!string.IsNullOrWhiteSpace(code) && code == "10")
            {
                ViewBag.Error = ValidationMessages.VTPInvalidAccount;
            }
            else if (code != "00")
            {
                ViewBag.Error = ValidationMessages.VTPError;
            }

            TempData["OrderId"] = order.Id.ToString();

            if (currentUser.WithdrawProcessing && (DateTime.Now - currentUser.WithdrawProcessingDate).Minutes <= 5)
            {
                ViewBag.Error = ValidationMessages.WithdrawalError;
            }
            else if(currentUser.WithdrawProcessing && (DateTime.Now - currentUser.WithdrawProcessingDate).Minutes > 5)
            {
                currentUser.WithdrawProcessing = false;

                await _userService.UpdateUser(currentUser);

            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Withdrawal(WithdrawalViewModel model)
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.WithdrawalAmount < _configuration.GetValue<decimal>("InvestmentValidation:MinWithdrawalAmount"))
            {
                ModelState.AddModelError("WithdrawalAmount", string.Format(ValidationMessages.WithdrawalInvalid, _configuration.GetValue<decimal>("InvestmentValidation:MinWithdrawalAmount")));
                return View(model);
            }

            var currentUser = await _userService.GetCurrentUser();

            var withdrawalFee = await _withdrawFeeService.GetFeeAmount((decimal)model.WithdrawalAmount, true);
            if (model.WithdrawalType == Common.WithdrawalType.Quick)
            {
                var quickWithdrawalFee = await _withdrawFeeService.GetQuickWithdrawalFee();
                var dateQuickWithdrawal = _configuration.GetValue<int>("Fee:DateQuickWithdrawal");
                withdrawalFee += (decimal)model.WithdrawalAmount * quickWithdrawalFee * dateQuickWithdrawal;
            }

            var personalIncomeFee = _configuration.GetValue<decimal>("Fee:PersonalIncomeFee") / 100;
            withdrawalFee += (decimal)model.WithdrawalAmount * personalIncomeFee;

            if (model.WithdrawalAmount + withdrawalFee > currentUser.CurrentAccountAmount)
            {
                ModelState.AddModelError("WithdrawalAmount", ValidationMessages.WithdrawalInvalid2);
                return View(model);
            }

            if (model.WithdrawalType == Common.WithdrawalType.Quick && model.WithdrawalAmount > currentUser.CurrentAccountAmount * 90 / 100)
            {
                ModelState.AddModelError("WithdrawalAmount", ValidationMessages.WithdrawalInvalid3);
                return View(model);
            }

            if (currentUser.WithdrawProcessing && (DateTime.Now - currentUser.WithdrawProcessingDate).Minutes <= 5)
            {
                ViewBag.Error = ValidationMessages.WithdrawalError;
                return View();
            }

            currentUser.WithdrawProcessing = true;
            currentUser.WithdrawProcessingDate = DateTime.Now;
            await _userService.UpdateUser(currentUser);

            if (model.WithdrawalType == Common.WithdrawalType.Quick)
            {
                var viettelPayApi = _configuration.GetValue<bool>("RequestPaymentLink:IsLive") ? _configuration.GetValue<string>("RequestPaymentLink:Live") : _configuration.GetValue<string>("RequestPaymentLink:Test");
                var cmd = _configuration.GetValue<string>("RequestPaymentParam:cmdRequest");
                var rsaPublicKey = _configuration.GetValue<string>("RSAKey:public");
                var rsaPrivateKey = _configuration.GetValue<string>("RSAKey:private");
                var rsaPublicKeyVTP = _configuration.GetValue<string>("RSAKey:VTPpublic");

                var rsa = new RSAHelper(RSAType.RSA, Encoding.UTF8, rsaPrivateKey, rsaPublicKeyVTP);
                var passwordEncrypt = rsa.Encrypt(_configuration.GetValue<string>("RequestPaymentParam:password"));

                var dataRequestPayment = new DataRequestPayment()
                {
                    msisdn = "84" + currentUser.PhoneNumber.Remove(0, 1),
                    customerName = currentUser.FullName,
                    transId = TempData["OrderId"].ToString(),
                    amount = ((decimal)model.WithdrawalAmount).ToString("0"),
                    smsContent = _configuration.GetValue<string>("RequestPaymentParam:smsContent"),
                    note = "Rut tien tu Savenow"
                };

                var soapDataRequestPayment = new SoapDataRequestPayment()
                {
                    username = _configuration.GetValue<string>("RequestPaymentParam:username"),
                    password = passwordEncrypt,
                    serviceCode = _configuration.GetValue<string>("RequestPaymentParam:serviceCode"),
                    orderId = TempData["OrderId"].ToString(),
                    totalTrans = "1",
                    totalAmount = ((decimal)model.WithdrawalAmount).ToString("0"),
                    transContent = _configuration.GetValue<string>("RequestPaymentParam:smsContent")
                };

                var code = _viettelPay.Request(viettelPayApi, cmd, rsaPublicKey, rsaPrivateKey, rsaPublicKeyVTP, dataRequestPayment, soapDataRequestPayment);

                if (!string.IsNullOrWhiteSpace(code) && code == "10")
                {
                    currentUser.WithdrawProcessing = false;
                    await _userService.UpdateUser(currentUser);
                    ViewBag.Error = ValidationMessages.VTPInvalidAccount;
                    return View();
                }
                else if (code != "00")
                {
                    currentUser.WithdrawProcessing = false;
                    await _userService.UpdateUser(currentUser);
                    ViewBag.Error = ValidationMessages.VTPError;
                    return View();
                }
            }

            await _withdrawFeeService.GetFeeAmount((decimal)model.WithdrawalAmount);

            await _fundTransactionHistoryService.Withdrawal(currentUser.UserName, (decimal)model.WithdrawalAmount, withdrawalFee, model.WithdrawalType);

            return RedirectToAction(nameof(AccountController.MyWallet), "Account");
        }

        [Route("GetWithdrawalFee")]
        [HttpPost]
        public async Task<IActionResult> GetWithdrawalFee(string amount, int type = 1)
        {
            var withdrawalFee = await _withdrawFeeService.GetFeeAmount(decimal.Parse(amount), true);

            if ((Common.WithdrawalType)type == Common.WithdrawalType.Quick)
            {
                var quickWithdrawalFee = await _withdrawFeeService.GetQuickWithdrawalFee();
                var dateQuickWithdrawal = _configuration.GetValue<int>("Fee:DateQuickWithdrawal");
                withdrawalFee += decimal.Parse(amount) * quickWithdrawalFee * dateQuickWithdrawal;
            }

            var personalIncomeFee = _configuration.GetValue<decimal>("Fee:PersonalIncomeFee") / 100;
            withdrawalFee += decimal.Parse(amount) * personalIncomeFee;

            return Json(withdrawalFee);
        }
    }
}
