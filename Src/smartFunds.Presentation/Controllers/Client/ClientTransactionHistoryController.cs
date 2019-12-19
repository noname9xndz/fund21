using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using smartFunds.Common;
using smartFunds.Common.Helpers;
using smartFunds.Model.Admin;
using smartFunds.Model.Common;
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Controllers.Client
{
    [Route("transaction-history")]
    public class ClientTransactionHistoryController : Controller
    {
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public ClientTransactionHistoryController(ITransactionHistoryService transactionHistoryService, IUserService userService, IConfiguration configuration)
        {
            _transactionHistoryService = transactionHistoryService;
            _userService = userService;
            _configuration = configuration;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            return View();
        }

        [Route("list")]
        [HttpPost]
        public async Task<IActionResult> List(int type = 0, int status = 0, string transactionDateFrom = null, string transactionDateTo = null, int pageIndex = 1, int pageSize = 10)
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var model = new ListTransactionHistoryModel();

            DateTime from = new DateTime();
            DateTime to = new DateTime();
            if (!string.IsNullOrWhiteSpace(transactionDateFrom) && !string.IsNullOrWhiteSpace(transactionDateTo) && DateTime.TryParseExact(transactionDateFrom, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out from) && DateTime.TryParseExact(transactionDateTo, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out to) && from > to)
            {
                return NoContent();
            }
            else
            {
                var searchModel = new SearchTransactionHistory()
                {
                    TransactionType = (TransactionType)type,
                    Status = (TransactionStatus)status,
                    TransactionDateFrom = transactionDateFrom,
                    TransactionDateTo = transactionDateTo
                };
                var currentUser = await _userService.GetCurrentUser();

                var size = pageSize > 0 ? pageSize : _configuration.GetValue<int>("PagingConfig:PageSize");
                var page = pageIndex > 0 ? pageIndex : 1;

                model = await _transactionHistoryService.GetListTransactionHistory(size, page, currentUser.Id, searchModel);
                model.PageIndex = page;
                model.PageSize = size;
            }

            return PartialView("Views/ClientTransactionHistory/TransactionHistoryPartial.cshtml", model);
        }
    }
}
