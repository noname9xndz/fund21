using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using smartFunds.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using smartFunds.Common;
using smartFunds.Common.Helpers;
using smartFunds.Infrastructure.Models;
using smartFunds.Model.Admin;
using smartFunds.Model.Common;
using smartFunds.Model.Resources;
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;
using static smartFunds.Common.Constants;
using Newtonsoft.Json;
using smartFunds.Business.Common;
using Hangfire;
using System.Threading;

namespace smartFunds.Presentation.Controllers.Admin
{
    [Route("admin/task")]
    public class TaskController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ITaskService _taskService;
        private readonly ITaskCompletedService _taskCompletedService;
        private readonly IFundService _fundService;
        private readonly IConfiguration _configuration;
        private readonly IFundTransactionHistoryService _fundTransactionHistoryService;
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly IUserService _userService;
        private readonly IPortfolioService _portfolio;
        private readonly IViettelPay _viettelPay;
        private readonly IOrderRequestService _orderRequestService;
        private readonly IGlobalConfigurationService _globalConfigurationService;

        public TaskController(ITaskService taskService, IConfiguration configuration, IFundService fundService, IFundTransactionHistoryService fundTransactionHistoryService,
            IEmailSender emailSender, IUserService userService, ITransactionHistoryService transactionHistoryService, ITaskCompletedService taskCompletedService,
            IPortfolioService portfolio, IViettelPay viettelPay, IOrderRequestService orderRequestService, IGlobalConfigurationService globalConfigurationService)
        {
            _taskService = taskService;
            _configuration = configuration;
            _fundService = fundService;
            _fundTransactionHistoryService = fundTransactionHistoryService;
            _emailSender = emailSender;
            _userService = userService;
            _transactionHistoryService = transactionHistoryService;
            _taskCompletedService = taskCompletedService;
            _portfolio = portfolio;
            _viettelPay = viettelPay;
            _orderRequestService = orderRequestService;
            _globalConfigurationService = globalConfigurationService;
        }

        #region Task for admin
        [Authorize(Policy = "OnlyAdminAccess")]
        [Route("list-admin-task")]
        public async Task<IActionResult> ApproveList(int pageSize = 0, int pageIndex = 0)
        {
            TasksApproveModel model = new TasksApproveModel();
            var config = await _globalConfigurationService.GetConfig(Constants.Configuration.IsAdminApproving);
            ViewBag.IsAdminApproving = bool.Parse(config.Value);
            model.Tasks = await _taskService.GetTasksForAdmin(pageSize, pageIndex);
            return View(model);
        }

        [Authorize(Policy = "OnlyAdminAccess")]
        [Route("approved")]
        [HttpPost]
        public async Task<IActionResult> ApprovedTask(int idTask, TaskApproveAdmin typeTask)
        {
            await _globalConfigurationService.SetValueConfig(Constants.Configuration.IsAdminApproving, "true");
            BackgroundJob.Schedule(() => this.DoApprovedTask(idTask, typeTask), TimeSpan.FromSeconds(1));
            //TempData["Message"] = Model.Resources.ValidationMessages.ApproveInprogress;
            return Json(new { success = true });
        }

        public async Task DoApprovedTask(int idTask, TaskApproveAdmin typeTask)
        {
            try
            {
                if (typeTask == TaskApproveAdmin.Portfolio)
                {
                    await _fundTransactionHistoryService.ApproveFundPercent(idTask);
                }
                else if (typeTask == TaskApproveAdmin.Nav)
                {
                    await _fundService.UpdateApprovedFunds();
                }
            }
            finally
            {
                await _globalConfigurationService.SetValueConfig(Constants.Configuration.IsAdminApproving, "false");
            }

        }
        [Authorize(Policy = "OnlyAdminAccess")]
        [Route("rejected/{idTask}")]
        [HttpGet]
        public IActionResult RejectedTask(int idTask, TaskApproveAdmin typeTask)
        {
            var model = new TaskApproveModel();
            model = _taskService.GetTaskById(idTask, typeTask).Result;
            model.TaskStatus = TaskStatusAdmin.Rejected;
            model.TaskType = typeTask;
            return View(model);
        }

        [Route("rejectedtasks")]
        [HttpPost]
        public async Task<IActionResult> RejectedTasks(TaskApproveModel model)
        {
            if (model == null) return Json(false, new JsonSerializerSettings());
            // update data status
            if (model.TaskType == TaskApproveAdmin.Portfolio)
            {
                await _portfolio.RejectedPortfolio(model.IdTask);
            }
            else if (model.TaskType == TaskApproveAdmin.Nav)
            {
                await _fundService.UpdateApprovedFunds(false);
            }

            // gửi mail
            if (!string.IsNullOrEmpty(model.LastUpdatedBy) && model.LastUpdatedBy != "Unknown")
            {
                var fromer = _userService.GetUserByName(model.LastUpdatedBy).Result;
                if (!string.IsNullOrEmpty(fromer.Email))
                {
                    var sender = _userService.GetCurrentUser().Result.Email;
                    var contentEmail = _configuration.GetValue<string>("EmailBody:TaskAdmin").Replace("[FullName]", fromer.FullName).Replace("[TaskType]", model.NameTask).Replace("[ContentMessage]", model.ContentMessage).Replace("[Approveder]", sender);

                    var mailConfig = SetMailConfig(sender, fromer.Email, _configuration.GetValue<string>("EmailSubject:TaskAdmin"), contentEmail);
                    var sendEmail = _emailSender.SendEmail(mailConfig);
                    if (sendEmail)
                        ViewData["Message"] = ValidationMessages.SendMailSucess;
                    else
                        ViewData["Message"] = ValidationMessages.SendMailError;
                }
            }
            return RedirectToAction("ApproveList");
        }
        #endregion

        [Authorize(Policy = "AdminAccountantAccess")]
        [Route("list-accountant-task")]
        [HttpGet]
        public async Task<IActionResult> List(int pageSize = 0, int pageIndex = 0)
        {
            var size = pageSize > 0 ? pageSize : _configuration.GetValue<int>("PagingConfig:PageSize");
            var page = pageIndex > 0 ? pageIndex : 1;

            // Giao dịch quỹ
            var modelFund = new SearchBalanceFund();
            modelFund.Status = EditStatus.Updating;
            var listFundTransactionHistory = _fundTransactionHistoryService.GetsFundTransactionHistory(page, size, modelFund).Result;
            var fundTransactionHistoryModel = new ListFundTransactionHistoryModel();
            fundTransactionHistoryModel = listFundTransactionHistory;
            fundTransactionHistoryModel.PageIndex = page;
            fundTransactionHistoryModel.PageSize = size;
            ViewBag.FundTransactionHistoryModel = fundTransactionHistoryModel;
            ViewBag.SearchBalanceFund = new SearchBalanceFund() { AmountFrom = null, AmountTo = null, Funds = listFundTransactionHistory.ListFundTransactionHistory.Select(x => x.Fund) };

            // Giao dịch khách hàng
            var transactionHistoryModel = await _transactionHistoryService.GetListTransactionHistoryForTask(size, page, searchTransactionHistory: new SearchTransactionHistory());
            transactionHistoryModel.PageIndex = page;
            transactionHistoryModel.PageSize = size;
            ViewBag.TransactionHistoryModel = transactionHistoryModel;

            // Task hoàn thành
            var model = new TasksCompletedModel();
            model.TasksCompleted = new List<TaskCompletedModel>();
            model.PageIndex = page;
            model.PageSize = size;
            ViewBag.TaskModel = model;
            ViewBag.SearchTaskModel = new SearchTask();

            return View();
        }

        #region Pending Deal Customer
        [Authorize(Policy = "AdminAccountantAccess")]
        [Route("pending-deal-custom-list")]
        [HttpGet]
        public async Task<IActionResult> PendingDealCustomList(int pageSize = 0, int pageIndex = 0)
        {
            var size = pageSize > 0 ? pageSize : _configuration.GetValue<int>("PagingConfig:PageSize");
            var page = pageIndex > 0 ? pageIndex : 1;

            ViewBag.SearchTransactionHistoryViewModel = new SearchTransactionHistory();

            var model = new ListTransactionHistoryModel();
            model.PageIndex = page;
            model.PageSize = size;

            return PartialView("Views/Task/ListPendingDealCustomPartial.cshtml", model);
        }

        [Authorize(Policy = "AdminAccountantAccess")]
        [Route("pending-deal-custom-list")]
        [HttpPost]
        public async Task<IActionResult> PendingDealCustomList([FromBody]SearchTransactionHistory model, int pageSize = 0, int pageIndex = 0)
        {
            var size = pageSize > 0 ? pageSize : _configuration.GetValue<int>("PagingConfig:PageSize");
            var page = pageIndex > 0 ? pageIndex : 1;

            var transactionHistoryModel = await _transactionHistoryService.GetListTransactionHistoryForTask(size, page, searchTransactionHistory: model);
            transactionHistoryModel.PageIndex = page;
            transactionHistoryModel.PageSize = size;

            return PartialView("Views/Task/ListPendingDealCustomPartial.cshtml", transactionHistoryModel);
        }

        [Authorize(Policy = "AdminAccountantAccess")]
        [Route("pending-deal-custom-export")]
        [HttpPost]
        public async Task<IActionResult> DealCustomExport(SearchTransactionHistory searchModel)
        {
            var fileContent = await _transactionHistoryService.ExportDealCustom(searchModel);
            return File(fileContent, "application/ms-excel", $"DealCustom.xlsx");
        }

        [Authorize(Policy = "AdminAccountantAccess")]
        [Route("update-deal-custom")]
        [HttpPost]
        public async Task<IActionResult> UpdateDealCustom(int objectID, string userID, decimal transactionAmount)
        {
            var user = await _userService.GetUserById(userID);
            var objectName = user.FullName;

            if (!_configuration.GetValue<bool>("PaymentSecurity:DisableVTP"))
            {
                var newOrder = new OrderRequestModel()
                {
                    PhoneNumber = "84" + user.PhoneNumber.Remove(0, 1),
                    FullName = user.FullName,
                    Amount = transactionAmount
                };
                var order = await _orderRequestService.SaveOrder(newOrder);

                var viettelPayApi = _configuration.GetValue<bool>("RequestPaymentLink:IsLive") ? _configuration.GetValue<string>("RequestPaymentLink:Live") : _configuration.GetValue<string>("RequestPaymentLink:Test");
                var cmd = _configuration.GetValue<string>("RequestPaymentParam:cmdRequest");
                var cmdCheckAccount = _configuration.GetValue<string>("RequestPaymentParam:cmdCheckAccount");
                var rsaPublicKey = _configuration.GetValue<string>("RSAKey:public");
                var rsaPrivateKey = _configuration.GetValue<string>("RSAKey:private");
                var rsaPublicKeyVTP = _configuration.GetValue<string>("RSAKey:VTPpublic");

                var rsa = new RSAHelper(RSAType.RSA, Encoding.UTF8, rsaPrivateKey, rsaPublicKeyVTP);
                var passwordEncrypt = rsa.Encrypt(_configuration.GetValue<string>("RequestPaymentParam:password"));

                var dataCheckAccount = new DataCheckAccount()
                {
                    msisdn = "84" + user.PhoneNumber.Remove(0, 1),
                    customerName = user.FullName
                };

                var soapDataCheckAccount = new SoapDataCheckAccount()
                {
                    username = _configuration.GetValue<string>("RequestPaymentParam:username"),
                    password = passwordEncrypt,
                    serviceCode = _configuration.GetValue<string>("RequestPaymentParam:serviceCode"),
                    orderId = order.Id.ToString()
                };

                var codeCheckAccount = await _viettelPay.CheckAccount(viettelPayApi, cmdCheckAccount, rsaPublicKey, rsaPrivateKey, rsaPublicKeyVTP, dataCheckAccount, soapDataCheckAccount);

                if (!string.IsNullOrWhiteSpace(codeCheckAccount) && codeCheckAccount == "10")
                {
                    return Json(new { success = false, message = ValidationMessages.VTPInvalidAccount2 });
                }
                else if (codeCheckAccount != "00")
                {
                    return Json(new { success = false, message = ValidationMessages.VTPError });
                }

                var dataRequestPayment = new DataRequestPayment()
                {
                    msisdn = "84" + user.PhoneNumber.Remove(0, 1),
                    customerName = user.FullName,
                    transId = order.Id.ToString(),
                    amount = transactionAmount.ToString("0"),
                    smsContent = _configuration.GetValue<string>("RequestPaymentParam:smsContent"),
                    note = "Rut tien tu Savenow"
                };

                var soapDataRequestPayment = new SoapDataRequestPayment()
                {
                    username = _configuration.GetValue<string>("RequestPaymentParam:username"),
                    password = passwordEncrypt,
                    serviceCode = _configuration.GetValue<string>("RequestPaymentParam:serviceCode"),
                    orderId = order.Id.ToString(),
                    totalTrans = "1",
                    totalAmount = transactionAmount.ToString("0"),
                    transContent = _configuration.GetValue<string>("RequestPaymentParam:smsContent")
                };

                var code = await _viettelPay.Request(viettelPayApi, cmd, rsaPublicKey, rsaPrivateKey, rsaPublicKeyVTP, dataRequestPayment, soapDataRequestPayment);

                if (!string.IsNullOrWhiteSpace(code) && code == "10")
                {
                    return Json(new { success = false, message = ValidationMessages.VTPInvalidAccount });
                }
                else if (code != "00")
                {
                    return Json(new { success = false, message = ValidationMessages.VTPError });
                }
            }

            await _transactionHistoryService.UpdateStatusTransactionHistory(objectID, TransactionStatus.Success);
            await _taskCompletedService.SaveTaskCompleted(new TaskCompletedModel() { ObjectID = objectID, ObjectName = objectName, TaskType = TaskTypeAccountant.DealCustomer, TransactionAmount = transactionAmount });

            return Json(new { success = true });
        }
        #endregion

        #region Pending Deal Fund

        [Authorize(Policy = "AdminAccountantAccess")]
        [Route("pending-deal-fund-list")]
        [HttpGet]
        public async Task<IActionResult> PendingDealFundList(int pageSize = 0, int pageIndex = 0)
        {
            var size = pageSize > 0 ? pageSize : _configuration.GetValue<int>("PagingConfig:PageSize");
            var page = pageIndex > 0 ? pageIndex : 1;
            var model = new SearchBalanceFund();
            model.Status = EditStatus.Updating;
            var listFundTransactionHistory = _fundTransactionHistoryService.GetsFundTransactionHistory(page, size, model).Result;
            var searchModel = new SearchBalanceFund() { AmountFrom = null, AmountTo = null, Funds = listFundTransactionHistory.ListFundTransactionHistory.Select(x => x.Fund) };
            ViewBag.SearchModel = searchModel;

            listFundTransactionHistory.PageIndex = page;
            listFundTransactionHistory.PageSize = size;

            return PartialView("Views/Task/ListPendingDealFundPartial.cshtml", listFundTransactionHistory);
        }

        [Authorize(Policy = "AdminAccountantAccess")]
        [Route("pending-deal-fund-list")]
        [HttpPost]
        public async Task<IActionResult> PendingDealFundList([FromBody]SearchBalanceFund model, int pageSize = 0, int pageIndex = 0)
        {
            var size = pageSize > 0 ? pageSize : _configuration.GetValue<int>("PagingConfig:PageSize");
            var page = pageIndex > 0 ? pageIndex : 1;
            model.Status = EditStatus.Updating;
            var models = await _fundTransactionHistoryService.GetsFundTransactionHistory(page, size, model);
            models.PageIndex = page;
            models.PageSize = size;

            return PartialView("Views/Task/ListPendingDealFundPartial.cshtml", models);
        }

        [Authorize(Policy = "AdminAccountantAccess")]
        [Route("pending-deal-fund-export")]
        [HttpPost]
        public async Task<IActionResult> DealFundExport(SearchBalanceFund searchModel)
        {
            var fileContent = await _fundTransactionHistoryService.ExportDealFund(searchModel);
            return File(fileContent, "application/ms-excel", $"DealFund.xlsx");
        }

        [Authorize(Policy = "AdminAccountantAccess")]
        [Route("update-deal-fund")]
        [HttpPost]
        public async Task<IActionResult> UpdateDealFund(int objectID, decimal transactionAmount)
        {
            await _fundTransactionHistoryService.StartBalancing(objectID);
            BackgroundJob.Schedule(() => this.updateDealFun(objectID, transactionAmount), TimeSpan.FromSeconds(0));
            //updateDealFun(objectID, transactionAmount);
            return Json(new { success = true });
        }

        public async Task updateDealFun(int objectID, decimal transactionAmount)
        {
            
            try
            {
                await _globalConfigurationService.SetValueConfig(Constants.Configuration.ProgramLocked, "true");
                Thread.Sleep(30 * 1000);

                var objectName = _fundService.GetFundById(objectID)?.Title;
                await _fundTransactionHistoryService.ApproveBalanceFund(objectID);
                await _taskCompletedService.SaveTaskCompleted(
                    new TaskCompletedModel()
                    {
                        ObjectID = objectID,
                        ObjectName = objectName,
                        TaskType = transactionAmount > 0 ? TaskTypeAccountant.Buy : TaskTypeAccountant.Sell,
                        TransactionAmount = transactionAmount
                    });
                SendMailAdmin(objectName, objectName + " : " + transactionAmount.ToString("N0"));
            }
            finally
            {
               await _globalConfigurationService.SetValueConfig(Constants.Configuration.ProgramLocked, "false");

            }


        }

        public async Task BatchUpdateDealFund()
        {
            try
            {
                await _globalConfigurationService.SetValueConfig(Constants.Configuration.ProgramLocked, "true");
                var fundList = (await _fundTransactionHistoryService.GetListBalanceFund(EditStatus.Updating));
                var funds = fundList.Where(h => Math.Abs((h.TotalInvestNoOfCertificates - h.TotalWithdrawnNoOfCertificates) * h.Fund.NAV) > 3000000).ToList();

                foreach (var fund in funds)
                {
                    var transactionAmount = (fund.TotalInvestNoOfCertificates - fund.TotalWithdrawnNoOfCertificates) * fund.Fund.NAV;
                    await updateDealFun(fund.Fund.Id, transactionAmount);
                }
            }
            finally
            {
                await _globalConfigurationService.SetValueConfig(Constants.Configuration.ProgramLocked, "false");
            }
        }
        #endregion

        #region Completed Tasks

        [Authorize(Policy = "AdminAccountantAccess")]
        [Route("completed-list")]
        [HttpGet]
        public async Task<IActionResult> CompletedList(int pageSize = 0, int pageIndex = 0)
        {
            var size = pageSize > 0 ? pageSize : _configuration.GetValue<int>("PagingConfig:PageSize");
            var page = pageIndex > 0 ? pageIndex : 1;

            var searchModel = new SearchTask()
            {
                Status = TransactionStatus.Success,
                Funds = _fundService.GetAllFund()
            };

            ViewBag.SearchTaskModel = searchModel;
            var model = await _taskCompletedService.GetTasksCompleted(size, page, searchModel);

            model.PageIndex = page;
            model.PageSize = size;

            return PartialView("Views/Task/CompletedList.cshtml", model);
        }

        [Authorize(Policy = "AdminAccountantAccess")]
        [Route("completed-list")]
        [HttpPost]
        public async Task<IActionResult> SearchCompletedTasks([FromBody]SearchTask model, int pageSize = 0, int pageIndex = 0)
        {
            var size = pageSize > 0 ? pageSize : _configuration.GetValue<int>("PagingConfig:PageSize");
            var page = pageIndex > 0 ? pageIndex : 1;

            var tasksModel = await _taskCompletedService.GetTasksCompleted(size, page, model);
            tasksModel.PageIndex = page;
            tasksModel.PageSize = size;

            return PartialView("Views/Task/CompletedList.cshtml", tasksModel);
        }

        [Authorize(Policy = "AdminAccountantAccess")]
        [Route("export-completed")]
        [HttpPost]
        public async Task<IActionResult> ExportCompletedTasks(SearchTask searchModel)
        {
            var fileContent = await _taskCompletedService.ExportTasksCompleted(searchModel);

            return File(fileContent, "application/ms-excel", $"Completed Tasks.xlsx");
        }
        #endregion Completed Tasks

        [Authorize(Policy = "OnlyAdminAccess")]
        [Route("ApproveFundPercent")]
        [HttpGet]
        public async Task<IActionResult> ApproveFundPercent(int portfolioId)
        {
            await _fundTransactionHistoryService.ApproveFundPercent(portfolioId);
            var id = portfolioId;
            return RedirectToAction(nameof(PortfolioController.Detail), "Portfolio", new { id });
        }

        private MailConfig SetMailConfig(string from, string to, string subject, string body)
        {
            return new MailConfig()
            {
                EmailFrom = from,
                EnableSsl = _configuration.GetValue<bool>("EmailConfig:EnableSsl"),
                Port = _configuration.GetValue<int>("EmailConfig:Port"),
                Host = _configuration.GetValue<string>("EmailConfig:Host"),
                Username = _configuration.GetValue<string>("EmailConfig:Username"),
                Password = _configuration.GetValue<string>("EmailConfig:Password"),
                EmailSenderName = from,
                EmailSubject = subject,
                MailTo = to,
                EmailBody = body
            };
        }

        private void SendMailAdmin(string subjectBalance, string infoFunBalance)
        {
            var mailAdmin = _configuration.GetValue<string>("EmailConfig:AdminMail");
            var mailFrom = _configuration.GetValue<string>("EmailConfig:EmailFrom");
            var bodyValue = _configuration.GetValue<string>("EmailBody:BalanceNotice");
            var subject = _configuration.GetValue<string>("EmailBody:BalanceSubject");
            var mailConfig = SetMailConfig(mailFrom, mailAdmin, subject + subjectBalance, bodyValue + infoFunBalance + "đ");
            _emailSender.SendEmail(mailConfig);
        }

    }
}
