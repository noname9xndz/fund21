using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using smartFunds.Common;
using smartFunds.Common.Helpers;
using smartFunds.Model.Common;
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers.Admin
{
    [Authorize(Policy = "AdminManagerAccess")]
    [Route("admin/genericintroduce")]
    public class IntroducingPageCMSController : Controller
    {
        private readonly IIntroducingSettingService _introducingSettingService;
        public IntroducingPageCMSController(IIntroducingSettingService introducingSettingService)
        {
            _introducingSettingService = introducingSettingService;
        }

        [HttpGet]
        [Route("display")]
        public IActionResult Display()
        {
            var data = _introducingSettingService.GetSetting();
            if(data == null)
            {
                data = new GenericIntroducingSettingModel();
            }
            return View(data);
        }

        //public bool IsValidExtension(IFormFile File)
        //{
        //    string fileName = File.FileName;
        //    string extension = Path.GetExtension(fileName).ToLower();
        //    if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif")
        //        return true;
        //    else return false;
        //}

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add(GenericIntroducingSettingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isValidType = false;
                    
                    if (model.BannerFile != null)
                    {
                        ViewBag.ErrorDataImage = "Banner is missing";
                        isValidType = true;
                    }
                    if (!isValidType)
                    {
                        ModelState.AddModelError("Banner", Model.Resources.ValidationMessages.WrongFileType);
                        return Json(false, new JsonSerializerSettings());
                    }
                    await _introducingSettingService.AddDefault(model);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction(nameof(IntroducingPageCMSController.Display));
        }

        [HttpGet]
        [Route("update")]
        public  IActionResult Update()
        {
            IntroducingGeneralViewModel data = new IntroducingGeneralViewModel();
            var model = _introducingSettingService.GetSetting();
            if (model != null)
            {
                data.Banner = model.Banner;
                data.MobileBanner = model.MobileBanner;
                data.Description = model.Description;
            }
            return View(data);
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(IntroducingGeneralViewModel model)
        {
            
            if (!ModelState.IsValid)
            {
                return View();
            }
            int isDeleteMobile = 0;
            int isDeleteDesktop = 0;
            var data = _introducingSettingService.GetSetting();
            GenericIntroducingSettingModel introducingSettingModel = new GenericIntroducingSettingModel();
            introducingSettingModel.BannerFile = model.BannerFile;
            introducingSettingModel.MobileBannerFile = model.MobileBannerFile;
            introducingSettingModel.Description = model.Description;
            isDeleteDesktop = int.Parse(model.IsDeleteDesktopBanner);
            isDeleteMobile = int.Parse(model.IsDeleteMobileBanner);
            if (data == null)
            {
                await Add(introducingSettingModel);
            }
            else
            {
                introducingSettingModel.Id = data.Id;
                _introducingSettingService.UpdateIntroducingSetting(introducingSettingModel, isDeleteMobile, isDeleteDesktop);
            }
            return RedirectToAction("Index", "SettingsCMS", null);
        }

        
    }
}