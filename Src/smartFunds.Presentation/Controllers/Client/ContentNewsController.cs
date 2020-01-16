using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using smartFunds.Common;
using smartFunds.Common.Helpers;
using smartFunds.Model.Common;
using smartFunds.Model.Resources;
using smartFunds.Service.Services;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Controllers.Client
{
    [Route("ContentNews")]
    public class ContentNewsController : Controller
    {
        private readonly IContentNewsService _newsService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public ContentNewsController(IUserService userService, IContentNewsService newsService, IConfiguration configuration)
        {
            _userService = userService;
            _newsService = newsService;
            _configuration = configuration;
        }

       
       
        [Route("ListNews")]
        [HttpGet]

        public async Task<IActionResult> ListNews()
        {
            try
            {
                //if (!_userService.IsSignedIn(User))
                //{
                //    return RedirectToAction(nameof(HomeController.Index), "Home");
                //}
                //get contact from db
                LstNewsModel model = new LstNewsModel();
                model = await _newsService.GetListContentNewsPaging(10,1,1);


                return View(model);
               // return View("~/Views/Support/Contact.cshtml", contact);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Route("Detail")]
        [HttpGet]
      
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                //get contact from db
                //if (!_userService.IsSignedIn(User))
                //{
                //    return RedirectToAction(nameof(HomeController.Index), "Home");
                //}
                ContentNewsModel model = new ContentNewsModel();
                model = await _newsService.GetContentNews(id);

                ViewBag.LstNewsOthersViewModel = await _newsService.GetListContentNewsOtherNew(5, 1, id, model.PostDate);
                ViewBag.LstNewsOthersViewModelOld= await _newsService.GetListContentNewsOtherOld(5, 1, id, model.PostDate);
                return View(model);
                // return View("~/Views/Support/Contact.cshtml", model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
