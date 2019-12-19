using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using smartFunds.Common;
using smartFunds.Common.Exceptions;
using smartFunds.Common.Helpers;
using smartFunds.Model.Common;
using smartFunds.Model.Resources;
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers
{
    [Authorize(Policy = "AdminManagerAccess")]
    [Route("admin/check-fund")]
    public class CheckFundController : Controller
    {
        private readonly IFundService _fundService;
        private readonly IFundTransactionHistoryService _fundTransactionHistoryService;
        private readonly IConfiguration _configuration;

        public CheckFundController(IFundService fundService, IConfiguration configuration, IFundTransactionHistoryService fundTransactionHistoryService)
        {
            _fundService = fundService;
            _configuration = configuration;
            _fundTransactionHistoryService = fundTransactionHistoryService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Detail()
        {
            var model = new CheckFundViewModel();
            model.CountUserFund = await _fundTransactionHistoryService.GetCountUserFund(0);
            model.TotalAmountUserFund = await _fundTransactionHistoryService.GetTotalAmountInvestFund(0);
            return View(model);
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Detail(CheckFundViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            model.CountUserFund = await _fundTransactionHistoryService.GetCountUserFund(int.Parse(model.FundId));
            model.TotalAmountUserFund = await _fundTransactionHistoryService.GetTotalAmountInvestFund(int.Parse(model.FundId));

            return View(model);
        }

        [Route("GetInvestmentFunds")]
        [HttpGet]
        public async Task<IActionResult> GetInvestmentFunds()
        {
            var model = await _fundTransactionHistoryService.GetInvestmentFunds();

            return Ok(model);
        }
    }
}
