using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using smartFunds.Common;
using smartFunds.Infrastructure.Models;
using smartFunds.Infrastructure.Services;
using smartFunds.Model.Common;
using smartFunds.Model.Resources;
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers.Client
{
    [Route("api")]
    public class VerifyApiController : Controller
    {
        private readonly IUserService _userService;
        private readonly IFundTransactionHistoryService _fundTransactionHistoryService;
        private readonly InvestmentTargetService _investmentTargetService;
        private readonly IConfiguration _configuration;
        private readonly ISMSGateway _smsGateway;
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly IOrderService _orderService;
        private readonly IGlobalConfigurationService _globalConfigurationService;

        public VerifyApiController(IUserService userService, IFundTransactionHistoryService fundTransactionHistoryService, 
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

        [Route("payment/VerifyData")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyData(string billcode, string merchant_code, string order_id, string check_sum)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            logger.Info("Investment with ViettelPay | Verify Data: billcode: " + billcode + ", merchant_code: " + merchant_code + ", order_id: " + order_id + ", check_sum: " + check_sum);

            var orderVerify = new OrderVerifyModel()
            {
                billcode = billcode,
                check_sum = check_sum?.Replace(" ", "+").Replace("=", "%3D").Replace("+", "%2B"),
                order_id = order_id,
                merchant_code = merchant_code
            };

            try
            {
                var orderId = 0;
                var config = await _globalConfigurationService.GetValueConfig(Constants.Configuration.ProgramLocked);
                if (string.IsNullOrWhiteSpace(billcode) || string.IsNullOrWhiteSpace(merchant_code) || !Int32.TryParse(order_id, out orderId) || string.IsNullOrWhiteSpace(check_sum) || config.Contains("true"))
                {
                    orderVerify.error_code = "01";
                    logger.Info("Investment with ViettelPay | Verify Data: error_code: 01");

                    return Json(orderVerify);
                }

                var order = await _orderService.GetOrder(orderId);

                if (order != null && !order.IsSuccess && !order.IsVerify)
                {
                    var checkSum = Helpers.CreateCheckSum(_configuration.GetValue<string>("PaymentSecurity:AccessCode"), _configuration.GetValue<string>("PaymentSecurity:SecretKey"),
                   order.Id.ToString(), order.MerchantCode, order.Id.ToString(), order.TransAmount.ToString());
                    if (orderVerify.check_sum == checkSum)
                    {
                        orderVerify.error_code = "00";
                        orderVerify.trans_amount = order.TransAmount;

                        logger.Info("Investment with ViettelPay | Verify Data: error_code: 00");

                        order.IsVerify = true;
                        await _orderService.UpdateOrder(order);

                        return Json(orderVerify);
                    }
                }

                orderVerify.error_code = "02";
                logger.Info("Investment with ViettelPay | Verify Data: error_code: 02");
                return Json(orderVerify);
            }
            catch (Exception)
            {
                orderVerify.error_code = "03";
                logger.Info("Investment with ViettelPay | Verify Data: error_code: 03");
                return Json(orderVerify);
            }
        }

        [Route("payment/GetResult")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetResult(string billcode, string cust_msisdn, string error_code, string merchant_code, string order_id, string payment_status, string trans_amount, string vt_transaction_id, string check_sum)
        {
            var paymentResult = new PaymentResultModel()
            {
                error_code = "00",
                merchant_code = merchant_code,
                order_id = order_id,
                return_url = Url.Action(action: nameof(AccountController.MyWallet), controller: "Account", values: null, protocol: Request.Scheme),
                return_bill_code = billcode,
                return_other_info = "",
                check_sum = Helpers.CreateCheckSum(_configuration.GetValue<string>("PaymentSecurity:AccessCode"), _configuration.GetValue<string>("PaymentSecurity:SecretKey"),
                   "00", merchant_code, order_id)
            };

            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            logger.Info("Investment with ViettelPay: billcode = " + billcode + ", cust_msisdn = " + cust_msisdn + ", error_code = " + error_code + ", merchant_code = " + merchant_code + ", order_id = " + order_id + ", payment_status = " + payment_status + ", trans_amount = " + trans_amount + ", vt_transaction_id = " + vt_transaction_id + ", check_sum = " + check_sum);

            var orderId = 0;
            var config = await _globalConfigurationService.GetValueConfig(Constants.Configuration.ProgramLocked);
            if (string.IsNullOrWhiteSpace(error_code) || string.IsNullOrWhiteSpace(cust_msisdn) || string.IsNullOrWhiteSpace(merchant_code) || string.IsNullOrWhiteSpace(order_id) || !Int32.TryParse(order_id, out orderId) || string.IsNullOrWhiteSpace(payment_status) || string.IsNullOrWhiteSpace(check_sum) || config.Contains("true"))
            {

                logger.Error("Investment with ViettelPay: Param Invalid");

                paymentResult.error_code = "01";
                paymentResult.check_sum = Helpers.CreateCheckSum(_configuration.GetValue<string>("PaymentSecurity:AccessCode"), _configuration.GetValue<string>("PaymentSecurity:SecretKey"),
                   "01", merchant_code, order_id);

                return Json(paymentResult);
            }

            var phone = cust_msisdn;
            var userName = "0" + phone.Remove(0, 2);
            var currentUser = await _userService.GetUserByName(userName);

            if (currentUser == null || currentUser.KVRR == null)
            {
                logger.Error("Investment with ViettelPay: Error: User Not Found: " + userName);

                paymentResult.error_code = "01";
                paymentResult.check_sum = Helpers.CreateCheckSum(_configuration.GetValue<string>("PaymentSecurity:AccessCode"), _configuration.GetValue<string>("PaymentSecurity:SecretKey"),
                   "01", merchant_code, order_id);

                return Json(paymentResult);
            }

            if (error_code == "00" && (Int32.Parse(payment_status) == 0 || Int32.Parse(payment_status) == 1 || Int32.Parse(payment_status) == 3))
            {
                var order = await _orderService.GetOrder(orderId);
                if (order == null || order.IsSuccess)
                {
                    logger.Error("Investment with ViettelPay: Error: Order Not Found ");
                    paymentResult.error_code = "01";
                    paymentResult.check_sum = Helpers.CreateCheckSum(_configuration.GetValue<string>("PaymentSecurity:AccessCode"), _configuration.GetValue<string>("PaymentSecurity:SecretKey"),
                       "01", merchant_code, order_id);
                    return Json(paymentResult);
                }

                var checkSum = Helpers.CreateCheckSum(_configuration.GetValue<string>("PaymentSecurity:AccessCode"), _configuration.GetValue<string>("PaymentSecurity:SecretKey"),
                order.Id.ToString(), cust_msisdn, error_code, order.MerchantCode, order.Id.ToString(), payment_status, order.TransAmount, vt_transaction_id);
                if (check_sum.Replace(" ", "+").Replace("=", "%3D").Replace("+", "%2B") != checkSum)
                {
                    order.IsSuccess = true;
                    await _orderService.UpdateOrder(order);

                    logger.Error("Investment with ViettelPay: Error: check_sum invalid");

                    paymentResult.error_code = "01";
                    paymentResult.check_sum = Helpers.CreateCheckSum(_configuration.GetValue<string>("PaymentSecurity:AccessCode"), _configuration.GetValue<string>("PaymentSecurity:SecretKey"),
                       "01", merchant_code, order_id);
                    return Json(paymentResult);
                }

                await _fundTransactionHistoryService.Investment(decimal.Parse(order.TransAmount), currentUser.UserName,order_id);
                if (order.IsInvestmentTarget)
                {
                    var investmentTarget = await _investmentTargetService.GetInvestmentTarget(currentUser.Id);
                    investmentTarget.Status = Common.EditStatus.Success;
                    await _investmentTargetService.UpdateInvestmentTarget(investmentTarget);

                    //if(investmentTarget.InvestmentMethod == Common.InvestmentMethod.Manually)
                    //{
                    //    var smsConfig = SetSMSConfig(currentUser.UserName, _configuration.GetValue<string>("SMSMessage:InvestmentTarget"));
                    //    if (investmentTarget.Frequency == Common.Frequency.OneMonth)
                    //    {
                    //        RecurringJob.AddOrUpdate<InvestmentTargetService>("InvestmentTargetSend" + currentUser.UserName, s => s.AutoSendSMSRemind(currentUser.UserName, smsConfig), Cron.Monthly(DateTime.Now.Day));
                    //    }
                    //    else if (investmentTarget.Frequency == Common.Frequency.OneWeek)
                    //    {
                    //        RecurringJob.AddOrUpdate<InvestmentTargetService>("InvestmentTargetSend" + currentUser.UserName, s => s.AutoSendSMSRemind(currentUser.UserName, smsConfig), Cron.Weekly(DateTime.Now.DayOfWeek));
                    //    }
                    //    else if (investmentTarget.Frequency == Common.Frequency.TwoWeek)
                    //    {
                    //        RecurringJob.AddOrUpdate<InvestmentTargetService>("InvestmentTargetSend" + currentUser.UserName, s => s.AutoSendSMSRemind(currentUser.UserName, smsConfig), Cron.DayInterval(14));
                    //    }
                    //}
                    //else
                    //{
                    //    
                    //}
                }

                order.IsSuccess = true;
                await _orderService.UpdateOrder(order);

                return Json(paymentResult);
            }
            else
            {
                logger.Error("Investment with ViettelPay: Error Code: " + error_code);

                paymentResult.error_code = "01";
                paymentResult.check_sum = Helpers.CreateCheckSum(_configuration.GetValue<string>("PaymentSecurity:AccessCode"), _configuration.GetValue<string>("PaymentSecurity:SecretKey"),
                   "01", merchant_code, order_id);
                return Json(paymentResult);
            }
        }
    }
}
