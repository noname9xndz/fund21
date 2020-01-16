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
using System.Threading.Tasks;
using smartFunds.Common;

namespace smartFunds.Presentation.Controllers.Client
{
    [Route("invest")]
    public class InvestmentController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFundTransactionHistoryService _fundTransactionHistoryService;
        private readonly InvestmentTargetService _investmentTargetService;
        private readonly IConfiguration _configuration;
        private readonly ISMSGateway _smsGateway;
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly IOrderService _orderService;
        private readonly IGlobalConfigurationService _globalConfigurationService;

        public InvestmentController(IUserService userService, IFundTransactionHistoryService fundTransactionHistoryService,
            InvestmentTargetService investmentTargetService, IConfiguration configuration, ISMSGateway smsGateway, 
            ITransactionHistoryService transactionHistoryService, IOrderService orderService, IGlobalConfigurationService globalConfigurationService)
        {
            _userService = userService;
            _fundTransactionHistoryService = fundTransactionHistoryService;
            _configuration = configuration;
            _investmentTargetService = investmentTargetService;
            _smsGateway = smsGateway;
            _transactionHistoryService = transactionHistoryService;
            _orderService = orderService;
            _globalConfigurationService = globalConfigurationService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Invest()
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

            if (currentUser.KVRR == null)
            {
                return RedirectToAction(nameof(AccountController.MyPortfolio), "Account");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Invest(InvestViewModel model)
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

            if (currentUser.KVRR == null)
            {
                return RedirectToAction(nameof(AccountController.MyPortfolio), "Account");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.TargetAmount <= _configuration.GetValue<decimal>("InvestmentValidation:MinInvestAmount"))
            {
                ModelState.AddModelError("TargetAmount", ValidationMessages.InvestInvalid);
                return View(model);
            }
            var orderModel = new OrderModel()
            {
                Desc = "Đầu tư Savenow",
                MerchantCode = _configuration.GetValue<string>("PaymentParam:merchant_code"),
                Msisdn = currentUser.PhoneNumber,
                TransAmount = model.TargetAmount.ToString(),
                Version = _configuration.GetValue<string>("PaymentParam:version")
            };

            var order = await _orderService.SaveOrder(orderModel);

            if (!_configuration.GetValue<bool>("PaymentSecurity:DisableVTP"))
            {
                var check_sum = Helpers.CreateCheckSum(_configuration.GetValue<string>("PaymentSecurity:AccessCode"), _configuration.GetValue<string>("PaymentSecurity:SecretKey"),
                    order.Id.ToString(), _configuration.GetValue<string>("PaymentParam:command"), order.MerchantCode, order.Id.ToString(), order.TransAmount, order.Version);

                var paymentLink = _configuration.GetValue<bool>("PaymentLink:IsLive") ? _configuration.GetValue<string>("PaymentLink:Live") : _configuration.GetValue<string>("PaymentLink:Test");

                var paymentParameters = string.Format("billcode={0}&command={1}&desc={2}&merchant_code={3}&sender_msisdn={4}&order_id={5}&return_url={6}&cancel_url={7}&trans_amount={8}&version={9}&check_sum={10}",
                    order.Id.ToString(), _configuration.GetValue<string>("PaymentParam:command"), "Dau tu SmartFunds", _configuration.GetValue<string>("PaymentParam:merchant_code"), currentUser.PhoneNumber,
                    order.Id.ToString(), Url.Action(action: nameof(InvestmentController.DoPayment), controller: "Investment", values: null, protocol: Request.Scheme), Url.Action(action: nameof(InvestmentController.CancelPayment), controller: "Investment", values: null, protocol: Request.Scheme),
                    model.TargetAmount, _configuration.GetValue<string>("PaymentParam:version"), check_sum);

                var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
                logger.Info("Investment with ViettelPay: Call payment link: " + paymentLink + paymentParameters);

                return Redirect(paymentLink + paymentParameters);
            }
            else
            {
                await _fundTransactionHistoryService.Investment(decimal.Parse(order.TransAmount), currentUser.UserName, order.Id.ToString());
                return RedirectToAction(nameof(AccountController.PaymentResult), "Account");
            }
        }

        [Route("doPayment")]
        [HttpGet]
        public async Task<IActionResult> DoPayment(string billcode, string cust_msisdn, string error_code, string merchant_code, string order_id, string payment_status, string trans_amount, string vt_transaction_id, string check_sum)
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await _userService.GetCurrentUser();

            if (currentUser.KVRR == null)
            {
                return RedirectToAction(nameof(AccountController.MyPortfolio), "Account");
            }

            var orderId = 0;

            if(string.IsNullOrWhiteSpace(error_code) || string.IsNullOrWhiteSpace(merchant_code) || string.IsNullOrWhiteSpace(order_id) || !Int32.TryParse(order_id, out orderId) || string.IsNullOrWhiteSpace(payment_status) || string.IsNullOrWhiteSpace(check_sum))
            {
                
                return RedirectToAction(nameof(InvestmentController.Invest), "Investment");
            }

            if(error_code == "00" && (Int32.Parse(payment_status) == 0 || Int32.Parse(payment_status) == 1 || Int32.Parse(payment_status) == 3))
            {
               
                return RedirectToAction(nameof(AccountController.MyWallet), "Account");
            }
            else
            {
                TempData["Error"] = ValidationMessages.ConnectVTPError;
                return RedirectToAction(nameof(InvestmentController.Invest), "Investment");
            }

            
        }

        [Route("cancelPayment")]
        [HttpGet]
        public async Task<IActionResult> CancelPayment(string billcode, string cust_msisdn, string error_code, string merchant_code, string order_id, string payment_status, string trans_amount, string vt_transaction_id, string check_sum)
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await _userService.GetCurrentUser();

            if (currentUser.KVRR == null)
            {
                return RedirectToAction(nameof(AccountController.MyPortfolio), "Account");
            }

            var orderId = 0;
            if (string.IsNullOrWhiteSpace(error_code) || string.IsNullOrWhiteSpace(merchant_code) || string.IsNullOrWhiteSpace(order_id) || !Int32.TryParse(order_id, out orderId) || string.IsNullOrWhiteSpace(payment_status) || string.IsNullOrWhiteSpace(check_sum))
            {
                return RedirectToAction(nameof(AccountController.MyWallet), "Account");
            }

            var order = await _orderService.GetOrder(orderId);
            if (order != null && !order.IsSuccess && !order.IsVerify)
            {
                await _orderService.DeleteOrder(order);
            }

            return RedirectToAction(nameof(AccountController.MyWallet), "Account");
        }

        private SMSConfig SetSMSConfig(string phone, string message)
        {
            return new SMSConfig()
            {
                APIKey = _configuration.GetValue<string>("SMSConfig:APIKey"),
                SecretKey = _configuration.GetValue<string>("SMSConfig:SecretKey"),
                IsFlash = _configuration.GetValue<bool>("SMSConfig:IsFlash"),
                BrandName = _configuration.GetValue<string>("SMSConfig:BrandName"),
                SMSType = _configuration.GetValue<int>("SMSConfig:SMSType"),
                Phone = phone,
                Message = message
            };
        }
    }
}
