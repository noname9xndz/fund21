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
using smartFunds.Common;

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
        private readonly IGlobalConfigurationService _globalConfigurationService;
        private static Dictionary<string, bool> WithdrawalProcessingUsers = new Dictionary<string, bool>();
        public WithdrawalController(IUserService userService, IFundTransactionHistoryService fundTransactionHistoryService, 
            IInvestmentTargetService investmentTargetService, IConfiguration configuration, ISMSGateway smsGateway, 
            IWithdrawFeeService withdrawFeeService, IViettelPay viettelPay, IOrderRequestService orderRequestService, IGlobalConfigurationService globalConfigurationService)
        {
            _userService = userService;
            _fundTransactionHistoryService = fundTransactionHistoryService;
            _configuration = configuration;
            _investmentTargetService = investmentTargetService;
            _smsGateway = smsGateway;
            _withdrawFeeService = withdrawFeeService;
            _viettelPay = viettelPay;
            _orderRequestService = orderRequestService;
            _globalConfigurationService = globalConfigurationService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Withdrawal()
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var config = await _globalConfigurationService.GetValueConfig(Constants.Configuration.ProgramLocked);
            if (config.Contains("true"))
            {
                return View("~/Views/Lock.cshtml");
            }

            var currentUser = await _userService.GetCurrentUser();

            //Lock if user is withdrawing in another machine

            if (WithdrawalProcessingUsers.ContainsKey(currentUser.UserName) && WithdrawalProcessingUsers[currentUser.UserName])
            {
                ViewBag.Error = ValidationMessages.WithdrawalError;
                return View();
            }


            if (!_configuration.GetValue<bool>("PaymentSecurity:DisableVTP"))
            {
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

                var code = await _viettelPay.CheckAccount(viettelPayApi, cmd, rsaPublicKey, rsaPrivateKey, rsaPublicKeyVTP, dataCheckAccount, soapDataCheckAccount);

                if (!string.IsNullOrWhiteSpace(code) && code == "10")
                {
                    ViewBag.Error = ValidationMessages.VTPInvalidAccount;
                }
                else if (code != "00")
                {
                    ViewBag.Error = ValidationMessages.VTPError;
                }

                TempData["OrderId"] = order.Id.ToString();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Withdrawal(WithdrawalViewModel model, string withdrawalAll)
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var config = await _globalConfigurationService.GetValueConfig(Constants.Configuration.ProgramLocked);
            if (config.Contains("true"))
            {
                return View("~/Views/Lock.cshtml");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.WithdrawalAmount < _configuration.GetValue<decimal>("InvestmentValidation:MinWithdrawalAmount"))
            {
                ViewData["Message"] = string.Format(ValidationMessages.WithdrawalInvalid, _configuration.GetValue<decimal>("InvestmentValidation:MinWithdrawalAmount"));
                ModelState.AddModelError("WithdrawalAmount", string.Format(ValidationMessages.WithdrawalInvalid, _configuration.GetValue<decimal>("InvestmentValidation:MinWithdrawalAmount")));
                return View(model);
            }

            if (!await GoogleRecaptchaHelper.IsReCaptchaPassedAsync(Request.Form["g-recaptcha-response"], _configuration.GetValue<string>("GoogleReCaptcha:SecretKey")))
            {
                ViewData["CaptchaInvalidMessage"] = ValidationMessages.CaptchaInvalidMessage;
                return View(model);
            }

            var currentUser = await _userService.GetCurrentUser();

            if (WithdrawalProcessingUsers.ContainsKey(currentUser.UserName) && WithdrawalProcessingUsers[currentUser.UserName])
            {
                ViewBag.Error = ValidationMessages.WithdrawalError;
                return View();
            }

            try
            {
                //Lock user of withdrawal
                if (WithdrawalProcessingUsers.ContainsKey(currentUser.UserName))
                    WithdrawalProcessingUsers[currentUser.UserName] = true;
                else
                    WithdrawalProcessingUsers.Add(currentUser.UserName, true);

                
                var amount = (decimal)model.WithdrawalAmount;
                if (!string.IsNullOrWhiteSpace(withdrawalAll))
                {
                    if (model.WithdrawalType == Common.WithdrawalType.Quick)
                    {
                        amount = currentUser.CurrentAccountAmount * 90 / 100;
                    }
                    else
                    {
                        amount = currentUser.CurrentAccountAmount;
                    }
                }

                var withdrawalFee = await _withdrawFeeService.GetFeeAmount(amount, true);

                var checkWithdrawal = _configuration.GetValue<bool>("ViewClient:Withdrawal") != null ? _configuration.GetValue<bool>("ViewClient:Withdrawal") : true;

                if (model.WithdrawalType == Common.WithdrawalType.Quick && checkWithdrawal)
                {
                    var quickWithdrawalFee = await _withdrawFeeService.GetQuickWithdrawalFee();
                    var dateQuickWithdrawal = _configuration.GetValue<int>("Fee:DateQuickWithdrawal");
                    withdrawalFee += amount * quickWithdrawalFee * dateQuickWithdrawal;
                }

                var personalIncomeFee = _configuration.GetValue<decimal>("Fee:PersonalIncomeFee") / 100;
                withdrawalFee += amount * personalIncomeFee;

                withdrawalFee = Decimal.Round(withdrawalFee, 0);

                if ((decimal)model.WithdrawalAmount + withdrawalFee > Decimal.Round(currentUser.CurrentAccountAmount, 0))
                {
                    ViewData["Message"] = ValidationMessages.WithdrawalInvalid2;
                    ModelState.AddModelError("WithdrawalAmount", ValidationMessages.WithdrawalInvalid2);
                    return View(model);
                }

                if (model.WithdrawalType == Common.WithdrawalType.Quick && model.WithdrawalAmount > Decimal.Round(currentUser.CurrentAccountAmount * 90 / 100, 0) && checkWithdrawal)
                {
                    ViewData["Message"] = ValidationMessages.WithdrawalInvalid3;
                    ModelState.AddModelError("WithdrawalAmount", ValidationMessages.WithdrawalInvalid3);
                    return View(model);
                }

                if (currentUser.WithdrawProcessing && (DateTime.Now - currentUser.WithdrawProcessingDate).Minutes <= 5)
                {
                    ViewBag.Error = ValidationMessages.WithdrawalError;
                    return View();
                }

                TempData["Message"] = Model.Resources.Common.WithdrawalResult;

                if (model.WithdrawalType == Common.WithdrawalType.Quick && checkWithdrawal)
                {
                    if (!_configuration.GetValue<bool>("PaymentSecurity:DisableVTP"))
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

                        var code = await _viettelPay.Request(viettelPayApi, cmd, rsaPublicKey, rsaPrivateKey, rsaPublicKeyVTP, dataRequestPayment, soapDataRequestPayment);

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
                    TempData["Message"] = Model.Resources.Common.WithdrawalQuickResult;
                }

                await _withdrawFeeService.GetFeeAmount(amount);

                if (!string.IsNullOrWhiteSpace(withdrawalAll))
                {
                    await _fundTransactionHistoryService.Withdrawal(currentUser.UserName, (decimal)model.WithdrawalAmount, withdrawalFee, model.WithdrawalType, true, objectId:TempData["OrderId"]?.ToString());
                }
                else
                {
                    await _fundTransactionHistoryService.Withdrawal(currentUser.UserName, (decimal)model.WithdrawalAmount, withdrawalFee, model.WithdrawalType, objectId: TempData["OrderId"]?.ToString());
                }

                if (TempData["OrderId"] != null)
                {
                    var order = await _orderRequestService.GetOrder(orderId: Int32.Parse(TempData["OrderId"].ToString()));
                    order.Amount = (decimal)model.WithdrawalAmount;
                    await _orderRequestService.UpdateOrder(order);
                }
            }
            finally
            {
                WithdrawalProcessingUsers.Remove(currentUser.UserName);
            }

            
            return RedirectToAction(nameof(AccountController.WithdrawalResult), "Account");
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

            return Json(Decimal.Round(withdrawalFee, 0));
        }

        [Route("WithdrawalAll")]
        [HttpPost]
        public async Task<IActionResult> WithdrawalAll(int type = 1)
        {
            var currentUser = await _userService.GetCurrentUser();

            if ((Common.WithdrawalType)type == Common.WithdrawalType.Quick)
            {
                var amountQuick = Decimal.Round(currentUser.CurrentAccountAmount * 90 / 100, 0);

                var withdrawalFee = await _withdrawFeeService.GetFeeAmount(amountQuick, true);

                var personalIncomeFee = _configuration.GetValue<decimal>("Fee:PersonalIncomeFee") / 100;
                withdrawalFee += amountQuick * personalIncomeFee;

                var quickWithdrawalFee = await _withdrawFeeService.GetQuickWithdrawalFee();
                var dateQuickWithdrawal = _configuration.GetValue<int>("Fee:DateQuickWithdrawal");
                withdrawalFee += amountQuick * quickWithdrawalFee * dateQuickWithdrawal;

                return Json(new
                {
                    amount = Decimal.Round(amountQuick, 0),
                    fee = Decimal.Round(withdrawalFee, 0)
                });
            }
            else
            {
                var withdrawalFee = await _withdrawFeeService.GetFeeAmount(currentUser.CurrentAccountAmount, true);

                var personalIncomeFee = _configuration.GetValue<decimal>("Fee:PersonalIncomeFee") / 100;
                withdrawalFee += currentUser.CurrentAccountAmount * personalIncomeFee;
                withdrawalFee = Decimal.Round(withdrawalFee, 0);

                return Json(new { amount = Decimal.Round(currentUser.CurrentAccountAmount - withdrawalFee, 0), fee = withdrawalFee });
            }
        }

        
    }
}
