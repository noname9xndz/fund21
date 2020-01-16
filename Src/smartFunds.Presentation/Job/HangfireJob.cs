using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using smartFunds.Common;
using smartFunds.Common.Helpers;
using smartFunds.Infrastructure.Models;
using smartFunds.Infrastructure.Services;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Job
{
    public class HangfireJob
    {
        private readonly IConfiguration _configuration;
        private readonly IViettelPay _viettelPay;
        private readonly IOrderRequestService _orderRequestService;
        private readonly IFundTransactionHistoryService _fundTransactionHistoryService;
        private readonly IGlobalConfigurationService _globalConfigurationService;
        public HangfireJob(IConfiguration configuration, IViettelPay viettelPay, IOrderRequestService orderRequestService, IFundTransactionHistoryService fundTransactionHistoryService, IGlobalConfigurationService globalConfigurationService)
        {
            _configuration = configuration;
            _viettelPay = viettelPay;
            _orderRequestService = orderRequestService;
            _fundTransactionHistoryService = fundTransactionHistoryService;
            _globalConfigurationService = globalConfigurationService;
        }

        public async Task CheckOrderRequestPendingTask()
        {
            try
            {
                var config = await _globalConfigurationService.GetValueConfig(Constants.Configuration.ProgramLocked);
                if (config.Contains("true"))
                {
                    return;
                }
                var listorder = await _orderRequestService.GetAllOrderRequestByStatus(OrderRequestStatus.Pending);

                if (listorder != null && listorder.Count > 0)
                {
                    var viettelPayApi = _configuration.GetValue<bool>("RequestPaymentLink:IsLive") ? _configuration.GetValue<string>("RequestPaymentLink:Live") : _configuration.GetValue<string>("RequestPaymentLink:Test");
                    var cmd = _configuration.GetValue<string>("RequestPaymentParam:cmdCheckOrderRequest");
                    var rsaPublicKey = _configuration.GetValue<string>("RSAKey:public");
                    var rsaPrivateKey = _configuration.GetValue<string>("RSAKey:private");
                    var rsaPublicKeyVTP = _configuration.GetValue<string>("RSAKey:VTPpublic");

                    var rsa = new RSAHelper(RSAType.RSA, Encoding.UTF8, "", rsaPublicKeyVTP);
                    var passwordEncrypt = rsa.Encrypt(_configuration.GetValue<string>("RequestPaymentParam:password"));
                    var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
                    logger.Info("Check Order Request Job : Start ");
                    foreach (var orderRequest in listorder)
                    {
                        if (orderRequest.Amount > 0)
                        {
                            logger.Info("Check Order Request Job : orderRequestId = " + orderRequest.Id);

                            SoapDataCheckOrderRequest request = new SoapDataCheckOrderRequest()
                            {
                                username = _configuration.GetValue<string>("RequestPaymentParam:username"),
                                password = passwordEncrypt,
                                serviceCode = _configuration.GetValue<string>("RequestPaymentParam:serviceCode"),
                                orderId = orderRequest.Id.ToString()
                            };
                            var response = await _viettelPay.CheckOrderRequest(viettelPayApi, cmd, rsaPublicKey, rsaPrivateKey, rsaPublicKeyVTP, request);

                            if (response != null)
                            {
                                logger.Info("Check Order Request Job : orderRequestId = " + orderRequest.Id + " batchErrorCode : " + response);
                                if (response == "DISB_SUCCESS")
                                {
                                    orderRequest.Status = OrderRequestStatus.Success;
                                    await _orderRequestService.UpdateOrder(orderRequest);
                                }
                                else if (response == "DISB_FAILED" || response == "DISB_TIMEOUT" || response == "CANCEL_DISB")
                                {
                                    orderRequest.Status = OrderRequestStatus.Failure;
                                    await _orderRequestService.UpdateOrder(orderRequest);
                                    await _fundTransactionHistoryService.WithdrawRollback(orderRequest.Amount, orderRequest.PhoneNumber);
                                }
                            }
                            else
                            {
                                logger.Info("Check Order Request Job : orderRequestId = " + orderRequest.Id + " ErrorCode != 0 ");
                                orderRequest.Status = OrderRequestStatus.Failure;
                                await _orderRequestService.UpdateOrder(orderRequest);
                                await _fundTransactionHistoryService.WithdrawRollback(orderRequest.Amount, orderRequest.PhoneNumber);
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                throw new ApplicationException("Check Order Request Error: " + e.Message);
            }
        }


    }
}
