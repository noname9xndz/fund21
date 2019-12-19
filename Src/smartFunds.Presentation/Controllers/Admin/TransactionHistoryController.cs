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
    [Route("admin/transaction-history")]    
    public class TransactionHistoryController : Controller
    {
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly IConfiguration _configuration;

        public TransactionHistoryController(ITransactionHistoryService transactionHistoryService, IConfiguration configuration)
        {
            _transactionHistoryService = transactionHistoryService;
            _configuration = configuration;
        }

        [Authorize(Policy = "AdminManagerAccess")]
        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> List(int pageSize = 0, int pageIndex = 0)
        {
            var size = pageSize > 0 ? pageSize : _configuration.GetValue<int>("PagingConfig:PageSize");
            var page = pageIndex > 0 ? pageIndex : 1;

            ViewBag.SearchTransactionHistoryViewModel = new SearchTransactionHistory();

            var model = new ListTransactionHistoryModel();
            //model = await _transactionHistoryService.GetListTransactionHistory(size, page);
            model.PageIndex = page;
            model.PageSize = size;

            return View(model);
        }

        [Authorize(Policy = "AdminManagerAccess")]
        [Route("list")]
        [HttpPost]
        public async Task<IActionResult> List([FromBody]SearchTransactionHistory model, int pageSize = 0, int pageIndex = 0)
        {
            var size = pageSize > 0 ? pageSize : _configuration.GetValue<int>("PagingConfig:PageSize");
            var page = pageIndex > 0 ? pageIndex : 1;

            var transactionHistoryModel = await _transactionHistoryService.GetListTransactionHistory(size, page, searchTransactionHistory: model);
            transactionHistoryModel.PageIndex = page;
            transactionHistoryModel.PageSize = size;

            return PartialView("Views/TransactionHistory/ListTransactionHistoryPartial.cshtml", transactionHistoryModel);
        }

        [Authorize(Policy = "CustomerManagerNotAccess")]
        [Route("export")]
        [HttpPost]
        public async Task<IActionResult> Export(SearchTransactionHistory searchModel)
        {
            var fileContent = await _transactionHistoryService.ExportTransactionHistory(searchModel);
            return File(fileContent, "application/ms-excel", $"TransactionHistory.xlsx");
        }
    }
}
