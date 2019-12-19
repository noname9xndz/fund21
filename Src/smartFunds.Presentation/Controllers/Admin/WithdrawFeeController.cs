using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using smartFunds.Model.Common;
using smartFunds.Model.Resources;
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers.Admin
{

    [Authorize(Policy = "AdminManagerAccess")]
    [Route("admin/withdrawfee")]
    public class WithdrawFeeController : Controller
    {

        private readonly IWithdrawFeeService _withdrawFeeService;
        private readonly IMaintainingFeeService _maintainingFeeService;
        private readonly IPortfolioService _portfolioService;
        private readonly IHomepageCMSService _homepageCMSService;
        private readonly IIntroducingSettingService _introducingSettingService;
        private readonly IContactCMSService _contactConfigurationService;
        private readonly IInvestmentTargetCMSService _investmentTargetCMSService;
        public WithdrawFeeController(
                IMaintainingFeeService maintainingFeeService,
                IPortfolioService portfolioService,
                IHomepageCMSService homepageCMSService,
                IIntroducingSettingService introducingSettingService,
                IContactCMSService contactConfigurationService,
                IWithdrawFeeService withdrawFeeService,
                IInvestmentTargetCMSService investmentTargetCMSService)
        {
            _maintainingFeeService = maintainingFeeService;
            _portfolioService = portfolioService;
            _homepageCMSService = homepageCMSService;
            _introducingSettingService = introducingSettingService;
            _contactConfigurationService = contactConfigurationService;
            _maintainingFeeService = maintainingFeeService;
            _withdrawFeeService = withdrawFeeService;
            _investmentTargetCMSService = investmentTargetCMSService;
        }
        // GET: WithdrawFee
        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var data = (await _withdrawFeeService.GetConfiguration()).ListFees;
            return View(data);
        }

        // GET: WithdrawFee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(WithdrawFeeModel data)
        {
            await _withdrawFeeService.AddConfiguration(data);
            return RedirectToAction(nameof(WithdrawFeeController.Index));
        }

        [Route("delete")]
        [HttpPost]
        public JsonResult Delete([FromBody]int[] listId)
        {
            _withdrawFeeService.DeleteConfigurationByIds(listId);
            return Json(new { success = true });
        }

        // POST: WithdrawFee/Edit/5
        [Route("save")]
        [HttpPost]
        public async Task<IActionResult> Edit(ListWithdrawFee model)
        {
            List<WithdrawFeeModel> newFees = new List<WithdrawFeeModel>();
            List<WithdrawFeeModel> existedFees = new List<WithdrawFeeModel>();
            bool isValid = true;
            if (model.QuickWithdrawFee.Percentage != 0)
            {
                model.ListFees.Add(model.QuickWithdrawFee);
            }
            else
            {
                isValid = false;
                TempData["Error_WithdrawFee"] = ValidationMessages.QuickWithdrawalIsEmpty;
                return ReturnWithError(model);
            }

            foreach (var item in model.ListFees)
            {
                
                if (item.TimeInvestmentBegin >= item.TimeInvestmentEnd)
                {
                    isValid = false;
                    TempData["Error_WithdrawFee"] = ValidationMessages.CompareMonthFromMonthTo;
                    return ReturnWithError(model);
                }
                else
                {
                    var dataExisted = CheckDataExisted(item.Id).Result;
                    if (!dataExisted)
                    {
                        newFees.Add(item);
                    }
                    else
                    {
                        existedFees.Add(item);
                    }
                }
            }
                
            if (newFees.Count > 0)
            {
                await _withdrawFeeService.AddListFees(newFees);
            }
            if (existedFees.Count > 0)
            {
                await _withdrawFeeService.UpdateConfigurations(existedFees);
            }
            return RedirectToAction("Index", "SettingsCMS");
        }

        public async Task<bool> CheckDataExisted(int id)
        {
            var quickWithdrawalFee = await _withdrawFeeService.GetQuickWithdrawalFeeItem();
            var result = (await _withdrawFeeService.GetConfiguration()).ListFees.Any(x => x.Id == id) || quickWithdrawalFee.Id == id;
            if (result)
                return true;
            else return false;
        }

        public IActionResult ReturnWithError(ListWithdrawFee model)
        {
            SettingCMSModel settingCMSModel = new SettingCMSModel();
            settingCMSModel.ListMaintainingFee = _maintainingFeeService.GetConfiguration();
            settingCMSModel.ListWithdrawFee = model;
            settingCMSModel.ContactCMSModel = _contactConfigurationService.GetContactConfiguration();
            settingCMSModel.GenericIntroducingSettingModel = _introducingSettingService.GetSetting();
            settingCMSModel.InvestmentTargetSettingModel.ListModels = _investmentTargetCMSService.GetAll();
            settingCMSModel.HomepageCMSModel.HomepageModels = _homepageCMSService.GetAll();
            ViewBag.ListPortfolio = _portfolioService.GetAllPortfolio().ToList();
            ViewBag.Data = _investmentTargetCMSService.GetAll().ToList();

            return View("Views/SettingsCMS/Index.cshtml", settingCMSModel);
        }
    }
}