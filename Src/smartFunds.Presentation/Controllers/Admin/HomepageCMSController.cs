using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using smartFunds.Business.Common;
using smartFunds.Model.Common;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers.Admin
{
    [Authorize(Policy = "OnlyAdminAccess")]
    [Route("admin/homepagecms")]
    public class HomepageCMSController : Controller
    {
        private readonly HomepageCMSService _homepageCMSService;

        public HomepageCMSController(HomepageCMSService homepageCMSService)
        {
            _homepageCMSService = homepageCMSService;
        }

        [Route("display")]
        public IActionResult Index()
        {
            HomepageCMSModel cmsModel = new HomepageCMSModel();
            cmsModel.HomepageModels = _homepageCMSService.GetAll();
            return View(cmsModel);
        }

        [HttpPost]
        [Route("save")]
        public async Task<IActionResult> UpdateBanner(IFormFile image, int category = 0, string typeUpload = "", int Id = 0, int isAdd = 0)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    bool isValidType = false;
                    if (image.Length > 0)
                    {
                        string fileName = image.FileName;
                        string extension = Path.GetExtension(fileName).ToLower();
                        if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif")
                            isValidType = true;
                        if (!isValidType)
                        {
                            ModelState.AddModelError("Banner", Model.Resources.ValidationMessages.WrongFileType);
                            return Json(false, new JsonSerializerSettings());
                        }
                        HomepageCMSModel homepageCMS = new HomepageCMSModel();
                        homepageCMS.Banner = image;
                        homepageCMS.Category = category.ToString();
                        if(isAdd != 1)
                        {
                            await _homepageCMSService.UpdateHomepageConfiguration(homepageCMS,category, typeUpload, Id);
                        }
                        else
                        {
                            await _homepageCMSService.AddImageConfiguration(homepageCMS, typeUpload);
                        }
                       
                    }
                    //await _homepageCMSService.UpdateHomepageConfiguration(homepageCMSModel);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Index", "SettingsCMS");
        }

        [HttpPost]
        [Route("addbanner")]
        public async Task<IActionResult> AddBanner([FromBody]HomepageCMSModel homepageCMSModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isValidType = false;
                    if (homepageCMSModel.Banner?.Length > 0)
                    {
                        string fileName = homepageCMSModel.Banner.FileName;
                        string extension = Path.GetExtension(fileName).ToLower();
                        if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif")
                            isValidType = true;
                        if (!isValidType)
                        {
                            ModelState.AddModelError("Banner", Model.Resources.ValidationMessages.WrongFileType);
                            return Json(false, new JsonSerializerSettings());
                        }
                        homepageCMSModel.ImageName = fileName;
                    }
                    await _homepageCMSService.AddImageConfiguration(homepageCMSModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction(nameof(HomepageCMSController.Index));
        }

        //HomepageCMS/Delete/id
        [HttpGet]
        [Route("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _homepageCMSService.Delete(id);
            return RedirectToAction("Index", "SettingsCMS");
        }
    }
}