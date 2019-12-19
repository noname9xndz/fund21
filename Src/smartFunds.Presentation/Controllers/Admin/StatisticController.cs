using System;
using System.Collections.Generic;
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

namespace smartFunds.Presentation.Controllers.Admin
{
    [Authorize(Policy = "AdminManagerAccess")]
    [Route("admin/statistic")]    
    public class StatisticController : Controller
    {
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly IConfiguration _configuration;

        public StatisticController(ITransactionHistoryService transactionHistoryService, IConfiguration configuration)
        {
            _transactionHistoryService = transactionHistoryService;
            _configuration = configuration;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new StatisticModel();

            model = await _transactionHistoryService.GetStatisticAsync();

            return View(model);
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Index(StatisticModel model)
        {
            var searchTransactionHistory = new SearchTransactionHistory()
            {
                TransactionDateFrom = model.TransactionDateFrom,
                TransactionDateTo = model.TransactionDateTo
            };

            model = await _transactionHistoryService.GetStatisticAsync(searchTransactionHistory);
            model.TransactionDateFrom = searchTransactionHistory.TransactionDateFrom;
            model.TransactionDateTo = searchTransactionHistory.TransactionDateTo;

            return View(model);
        }
    }
}
