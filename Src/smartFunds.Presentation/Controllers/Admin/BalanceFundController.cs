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
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers
{
    [Authorize(Policy = "AdminAccountantAccess")]
    [Route("admin/balance-fund")]
    public class BalanceFundController : Controller
    {
        private readonly IFundService _fundService;
        private readonly IFundTransactionHistoryService _fundTransactionHistoryService;
        private readonly IConfiguration _configuration;

        public BalanceFundController(IFundService fundService, IConfiguration configuration, IFundTransactionHistoryService fundTransactionHistoryService)
        {
            _fundService = fundService;
            _configuration = configuration;
            _fundTransactionHistoryService = fundTransactionHistoryService;
        }

        [Route("list")]
        public async Task<IActionResult> List()
        {
            try
            {
                List<FundTransactionHistoryModel> model = new List<FundTransactionHistoryModel>();

                model = (await _fundTransactionHistoryService.GetListBalanceFund(Common.EditStatus.Updating)).Where(i => i.TotalInvestNoOfCertificates != i.TotalWithdrawnNoOfCertificates).OrderBy(i => i.Fund.Code).ToList();

                return View(model);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Route("detail/{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var model = new BalanceFundDetailModel();
            var listFundTransactionHistoryModel = await _fundTransactionHistoryService.GetFundTransactionHistoryByFundId(id, Common.EditStatus.Updating);
            model.FundTransactionHistory = await _fundTransactionHistoryService.GetListBalanceFund(id, Common.EditStatus.Updating);
            model.ListFundTransactionHistoryModel = listFundTransactionHistoryModel;

            return View(model);
        }

        [Route("export")]
        [HttpPost]
        public async Task<IActionResult> Export()
        {
            var fileContent = await _fundTransactionHistoryService.ExportBalanceFund(Common.EditStatus.Updating);

            return File(fileContent, "application/ms-excel", $"BalaceFund.xlsx");
        }
    }
}
