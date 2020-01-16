using System;
using System.Collections.Generic;
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

namespace smartFunds.Presentation.Controllers.Admin
{
    [Route("admin/catenews")]
    public class CateNewsController : Controller
    {
        private readonly ICateNewsService _cateService;
        private readonly IConfiguration _configuration;

        public CateNewsController(ICateNewsService cateService, IConfiguration configuration)
        {
            _cateService = cateService;
            _configuration = configuration;
        }
        [Authorize(Policy = "AdminManagerAccess")]
        [Route("detail/{id}")]
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            CateNewsModel model = await _cateService.GetCateNews(id);
            return View(model);
        }

        [Authorize(Policy = "AdminManagerAccess")]
        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            CateNewssModel model = new CateNewssModel();
            model.CateNewss = await _cateService.GetListCateNews();
            return View(model);
        }

       
        [Authorize(Policy = "AccountantInvestmentManagerAccess")]
        [Route("new")]
        [HttpGet]
        public async Task<IActionResult> New()
        {
            return View();
        }

        [Route("new")]
        [HttpPost]
        public async Task<IActionResult> New(CateNewsModel faqModel)
        {
            if (ModelState.IsValid)
            {
                faqModel.Created = DateTime.Now;
                faqModel.DateLastUpdated= DateTime.Now;
                faqModel.ParentNewsID = 0;
                var savedFAQ = await _cateService.SaveCateNews(faqModel);
                return RedirectToAction(nameof(CateNewsController.List));
            }
            return View(faqModel);
        }

        #region Update Cate News
        [Authorize(Policy = "AccountantInvestmentManagerAccess")]
        [Route("edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            CateNewsModel model = await _cateService.GetCateNews(id);
            return View(model);
        }

        [Route("edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(CateNewsModel faqModel)
        {
            if (ModelState.IsValid)
            {
                await _cateService.UpdateCateNews(faqModel);
                var id = faqModel.Id;
                return RedirectToAction(nameof(CateNewsController.List));
            }
            return View(faqModel);
        }
        #endregion Update Cate News
        [Authorize(Policy = "AccountantInvestmentManagerAccess")]
        [Route("delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _cateService.DeleteCateNews(id);
            return RedirectToAction(nameof(CateNewsController.List));
        }
    }
}
