using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smartFunds.Model.Common;
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers.Admin
{
    [Authorize(Policy = "OnlyAdminAccess")]
    [Route("admin/investmentsetting")]
    public class InvestmentTargetCMSController : Controller
    {
        private readonly IInvestmentTargetCMSService _investmentTargetCMSService;
        private readonly IPortfolioService _portfolioService;

        public InvestmentTargetCMSController(IInvestmentTargetCMSService InvestmentTargetCMSService, IPortfolioService portfolioService)
        {
            _investmentTargetCMSService = InvestmentTargetCMSService;
            _portfolioService = portfolioService;
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task Add(InvestmentTargetSettingModel data)
        {
            await _investmentTargetCMSService.AddConfiguration(data);
        }

        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> save([FromBody]ListInvestTargetDataModel ListModels)
        {
            List<InvestmentTargetSettingModel> existedFees = _investmentTargetCMSService.GetListInvestmentTargetSetting().Result;
            foreach (var item in ListModels.ListModels)
            {
                if(item.Value < 0)
                {
                    TempData["Error_Investmentsetting"] = "Giá trị nhập không hợp lệ";
                    return RedirectToAction("Index", "SettingsCMS", TempData["Error_Investmentsetting"]);
                }
                foreach(var data in existedFees)
                {
                    if(data.Id == item.Id)
                    {
                        data.Value = item.Value;
                    }

                }

            }
            
            await _investmentTargetCMSService.UpdateConfigurations(existedFees);
            return RedirectToAction("Index", "SettingsCMS");
        }
        public bool CheckDataExisted(int id)
        {
            var result = _investmentTargetCMSService.GetAll().Any(x => x.Id == id);
            if (result)
                return true;
            else return false;
        }
    }
}