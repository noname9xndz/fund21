using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using smartFunds.Model.Common;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers.Client
{
    public class IntroducingPageCMSController : Controller
    {
        private readonly IntroducingSettingService _introducingSettingService;
        public IntroducingPageCMSController(IntroducingSettingService introducingSettingService)
        {
            _introducingSettingService = introducingSettingService;
        }
        public IActionResult Index()
        {
            GenericIntroducingSettingModel data = _introducingSettingService.GetSetting();
            return View(data);
        }
    }
}