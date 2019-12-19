using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using smartFunds.Model.Common;
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers.Admin
{
    [Authorize(Policy = "OnlyAdminAccess")]
    [Route("admin/settings")]
    public class SettingsCMSController : Controller
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IHomepageCMSService _homepageCMSService;
        private readonly IIntroducingSettingService _introducingSettingService;
        private readonly IContactCMSService _contactConfigurationService;
        private readonly IMaintainingFeeService _maintainingFeeService;
        private readonly IWithdrawFeeService _withdrawFeeService;
        private readonly IInvestmentTargetCMSService _investmentTargetCMSService;
        private readonly IConfiguration _configuration;
        private readonly IFundService _fundService;

        public SettingsCMSController
            (
                IPortfolioService portfolioService,
                IHomepageCMSService homepageCMSService,
                IIntroducingSettingService introducingSettingService,
                IContactCMSService contactConfigurationService,
                IMaintainingFeeService maintainingFeeService,
                IWithdrawFeeService withdrawFeeService,
                IInvestmentTargetCMSService investmentTargetCMSService,
                IConfiguration configuration,
                IFundService fundService
            )
        {
            _portfolioService = portfolioService;
            _homepageCMSService = homepageCMSService;
            _introducingSettingService = introducingSettingService;
            _contactConfigurationService = contactConfigurationService;
            _maintainingFeeService = maintainingFeeService;
            _withdrawFeeService = withdrawFeeService;
            _investmentTargetCMSService = investmentTargetCMSService;
            _configuration = configuration;
            _fundService = fundService;
        }
        [Route("Index")]
        public async Task<IActionResult> Index(SettingCMSModel settingCMSModel)
        {
            if(!settingCMSModel.HasError)
            {
                SettingCMSModel model = new SettingCMSModel();

                //Anh DMDT
                model.HomepageCMSModel = _homepageCMSService.GetAll().FirstOrDefault(x => x.Category == "2");
                if (model.HomepageCMSModel == null)
                {
                    model.HomepageCMSModel = new HomepageCMSModel();
                }
                //Trang chu
                model.HomepageCMSModel.HomepageModels = _homepageCMSService.GetAll();
                model.GenericIntroducingSettingModel = _introducingSettingService.GetSetting();
                model.ListMaintainingFee = GetMainTainingFees(model);
                model.ListWithdrawFee = await GetWithdrawFees(model);
                model.InvestmentTargetSettingModel.ListModels = await _investmentTargetCMSService.GetListInvestmentTargetSetting();
                ViewBag.ListPortfolio = _portfolioService.GetAllPortfolio().ToList();
                ViewBag.Data = _investmentTargetCMSService.GetAll().ToList();
                if (model.GenericIntroducingSettingModel == null)
                {
                    model.GenericIntroducingSettingModel = new GenericIntroducingSettingModel();
                }
                model.ContactCMSModel = _contactConfigurationService.GetContactConfiguration();

                ViewBag.ListFund = _fundService.GetAllFund().OrderBy(i => i.Code).ToList();

                return View(model);
            }
            return View(settingCMSModel);
        }

        public ListMaintainingFee GetMainTainingFees(SettingCMSModel model)
        {
            var item = _maintainingFeeService.GetConfiguration();
            return item;
        }

        public async Task<ListWithdrawFee> GetWithdrawFees(SettingCMSModel model)
        {
            var item = await _withdrawFeeService.GetConfiguration();
            return item;
        }
    }
}