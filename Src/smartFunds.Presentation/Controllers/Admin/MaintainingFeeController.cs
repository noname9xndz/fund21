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
    [Authorize(Policy = "AdminManagerAccess")]
    [Route("admin/maintainingfee")]
    public class MaintainingFeeController : Controller
    {
        private readonly IMaintainingFeeService _maintainingFeeService;
        private readonly IPortfolioService _portfolioService;
        private readonly IHomepageCMSService _homepageCMSService;
        private readonly IIntroducingSettingService _introducingSettingService;
        private readonly IContactCMSService _contactConfigurationService;
        private readonly IWithdrawFeeService _withdrawFeeService;
        private readonly IInvestmentTargetCMSService _investmentTargetCMSService;
        public MaintainingFeeController(
                IMaintainingFeeService maintainingFeeService,
                IPortfolioService portfolioService,
                IHomepageCMSService homepageCMSService,
                IIntroducingSettingService introducingSettingService,
                IContactCMSService contactConfigurationService,
                IWithdrawFeeService withdrawFeeService,
                IInvestmentTargetCMSService investmentTargetCMSService
            )
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
        // GET: MaintainingFee
        [HttpGet]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var data = _maintainingFeeService.GetConfiguration();
            return View(data);
        }

        // GET: MaintainingFee/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(MaintainingFeeModel data)
        {
            await _maintainingFeeService.AddConfiguration(data);
            return RedirectToAction(nameof(SettingsCMSController.Index));
        }


        // POST: MaintainingFee/Edit/5
        [Route("edit")]
        [HttpPost]
        //public async Task<IActionResult> Edit([FromBody]ListMaintainingFee model)
        public async Task<IActionResult> Edit(ListMaintainingFee model)
        {
            List<MaintainingFeeModel> newFees = new List<MaintainingFeeModel>();
            List<MaintainingFeeModel> existedFees = new List<MaintainingFeeModel>();
            bool isValid = true;

            foreach (var item in model.ListFees)
            {
                if(item.AmountTo == 0)
                {
                    TempData["Error"] = Model.Resources.ValidationMessages.DataIsEmpty;
                    return ReturnWithError(model);
                }
                if (item.AmountFrom >= item.AmountTo)
                {
                    isValid = false;
                    TempData["Error"] = Model.Resources.ValidationMessages.CompareAmountFromAmountTo;
                    return ReturnWithError(model);
                }
                else
                {
                    var dataExisted = CheckDataExisted(item.Id);
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
                var listNewFee = newFees;
                //addtional test in case of empty database then database should check validation while data in "newFees list"
                foreach (var item in newFees)
                {
                    listNewFee = newFees.Where(x => (x.AmountFrom != item.AmountFrom || x.AmountTo != item.AmountTo || x.Percentage != item.Percentage)).ToList();

                    var result = listNewFee.Where(x => (item.AmountFrom >= x.AmountFrom && item.AmountTo <= x.AmountTo)
                                || (item.AmountFrom >= x.AmountFrom && item.AmountFrom <= x.AmountTo && item.AmountTo > x.AmountTo)
                                || (item.AmountFrom < x.AmountFrom && item.AmountTo >= x.AmountFrom && item.AmountTo <= x.AmountTo)
                                || (item.AmountFrom < x.AmountFrom && item.AmountTo > x.AmountTo)).ToList();
                    if (result.Count > 0 || result.Any())
                    {
                        TempData["Error"] = Model.Resources.ValidationMessages.RangeAmountExisted;
                        isValid = false;
                    }

                }
                if (!isValid)
                {
                    return ReturnWithError(model);
                }
                await _maintainingFeeService.AddListFees(newFees);
            }
            if (existedFees.Count > 0)
            {
                await _maintainingFeeService.UpdateConfigurations(existedFees);
            }
            return RedirectToAction("Index", "SettingsCMS");
        }


        [Route("delete")]
        [HttpPost]
        public JsonResult Delete([FromBody]int[] listId)
        {            
            _maintainingFeeService.DeleteConfigurationByIds(listId);
            return Json(new { success = true });
        }

        public bool CheckDataExisted(int id)
        {
            var result = _maintainingFeeService.GetConfiguration().ListFees.Any(x => x.Id == id);
            if (result)
                return true;
            else return false;
        }

        public IActionResult ReturnWithError(ListMaintainingFee model)
        {
            SettingCMSModel settingCMSModel = new SettingCMSModel();
            settingCMSModel.ListMaintainingFee = model;
            settingCMSModel.ListWithdrawFee = _withdrawFeeService.GetConfiguration().Result;
            settingCMSModel.InvestmentTargetSettingModel.ListModels = _investmentTargetCMSService.GetAll();
            settingCMSModel.ContactCMSModel = _contactConfigurationService.GetContactConfiguration();
            settingCMSModel.GenericIntroducingSettingModel = _introducingSettingService.GetSetting();
            settingCMSModel.HomepageCMSModel.HomepageModels = _homepageCMSService.GetAll();
            ViewBag.ListPortfolio = _portfolioService.GetAllPortfolio().ToList();
            ViewBag.Data = _investmentTargetCMSService.GetAll().ToList();
            //settingCMSModel.HasError = true;
            //TempData["ErrorData"] = settingCMSModel;
            //return RedirectToAction("Index", "SettingsCMS");
            return View("Views/SettingsCMS/Index.cshtml", settingCMSModel);
        }



    }
}