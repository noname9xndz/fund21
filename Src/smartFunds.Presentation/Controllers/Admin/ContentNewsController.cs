using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using smartFunds.Common;
using smartFunds.Common.Helpers;
using smartFunds.Model.Common;
using smartFunds.Model.Resources;
using smartFunds.Service.Services;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Controllers.Admin
{
    [Route("admin/ContentNews")]
    public class ContentNewsController : Controller
    {
        private readonly IContentNewsService _newsService;
        private readonly IConfiguration _configuration;
        private readonly ICateNewsService _cateService;
        public ContentNewsController(IContentNewsService newsService, IConfiguration configuration)
        {
            _newsService = newsService;
            _configuration = configuration;
        }
       
        [Authorize(Policy = "AdminManagerAccess")]
        [Route("list")]
        [HttpGet]
       
        public async Task<IActionResult> List( int pageSize = 10, int pageIndex = 1)
        {
            var size = pageSize > 0 ? pageSize : _configuration.GetValue<int>("PagingConfig:PageSize");
            var page = pageIndex > 0 ? pageIndex : 1;

            var transactionHistoryModel = await _newsService.GetListContentNewsPaging(size, page,0);
            transactionHistoryModel.PageIndex = page;
            transactionHistoryModel.PageSize = size;

            return View( transactionHistoryModel);
        }


        [Authorize(Policy = "AdminManagerAccess")]
        [Route("list")]
        [HttpPost]

        public async Task<IActionResult> List(int pageSize = 10, int pageIndex = 1, string type = "", string status = "", string transactionDateFrom = "", string transactionDateTo = "")
        {
            type = "1";

            var size = pageSize > 0 ? pageSize : _configuration.GetValue<int>("PagingConfig:PageSize");
            var page = pageIndex > 0 ? pageIndex : 1;

            var transactionHistoryModel = await _newsService.GetListContentNewsPaging(size, page, 0);
            transactionHistoryModel.PageIndex = page;
            transactionHistoryModel.PageSize = size;

            return PartialView("Views/ContentNews/ListContentNewsPartial.cshtml", transactionHistoryModel);
        }
        [Authorize(Policy = "AccountantInvestmentManagerAccess")]
        [Route("new")]
        [HttpGet]
        public async Task<IActionResult> New()
        {
            CateNewssModel modelq = new CateNewssModel();
            modelq.CateNewss = await _newsService.GetAllCateNews();
           // ViewBag.CateNewsSelectList = data;
            ContentNewsModel faqModel = new ContentNewsModel();
            List<SelectListItem> CateNewsIDs = new List<SelectListItem>();
            foreach (CateNewsModel item in modelq.CateNewss)
            {

                SelectListItem item1 = new SelectListItem { Text = item.CateNewsName, Value = item.Id.ToString() };
                CateNewsIDs.Add(item1);
            }
            faqModel.CateNewsIDs = CateNewsIDs;
            faqModel.PostDate = DateTime.Now;

            return View(faqModel);
        }

        [Route("new")]
        [HttpPost]
        public async Task<IActionResult> New(ContentNewsModel faqModel)
        {
            if (ModelState.IsValid)
            {
                DateTime time = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(faqModel.TransactionDateFrom) && DateTime.TryParseExact(faqModel.TransactionDateFrom, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out time))
                {
                    faqModel.PostDate = time;

                    //   faqModel.Status = true;
                    bool isValidType = false;

                    if (faqModel.NewsImage == null) isValidType = true;
                    if (faqModel.NewsImage?.Length > 0)
                    {
                        var name = faqModel.NewsImage.FileName;
                        string fileName = name.Split('\\').Last();
                        string extension = Path.GetExtension(fileName).ToLower();
                        if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif")
                            isValidType = true;
                    }

                    if (!isValidType)
                    {
                        ModelState.AddModelError("NewsImage", Model.Resources.ValidationMessages.WrongFileType);
                        CateNewssModel modelq = new CateNewssModel();
                        modelq.CateNewss = await _newsService.GetAllCateNews();
                        // ViewBag.CateNewsSelectList = data;

                        List<SelectListItem> CateNewsIDs = new List<SelectListItem>();
                        foreach (CateNewsModel item in modelq.CateNewss)
                        {

                            SelectListItem item1 = new SelectListItem { Text = item.CateNewsName, Value = item.Id.ToString() };
                            CateNewsIDs.Add(item1);
                        }
                        faqModel.CateNewsIDs = CateNewsIDs;
                    }
                    var savedFAQ = await _newsService.SaveContentNews(faqModel);
                    return RedirectToAction(nameof(ContentNewsController.List));
               }
                else
                {


                    CateNewssModel modelq = new CateNewssModel();
                    modelq.CateNewss = await _newsService.GetAllCateNews();
                    // ViewBag.CateNewsSelectList = data;
                 
                    List<SelectListItem> CateNewsIDs = new List<SelectListItem>();
                    foreach (CateNewsModel item in modelq.CateNewss)
                    {

                        SelectListItem item1 = new SelectListItem { Text = item.CateNewsName, Value = item.Id.ToString() };
                        CateNewsIDs.Add(item1);
                    }
                    faqModel.CateNewsIDs = CateNewsIDs;

                }
            }
            return View(faqModel);
        }


        #region Update Content News
        [Authorize(Policy = "AccountantInvestmentManagerAccess")]
        [Route("edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ContentNewsModel model = await _newsService.GetContentNews(id);
            CateNewssModel modelq = new CateNewssModel();
            modelq.CateNewss = await _newsService.GetAllCateNews();
            // ViewBag.CateNewsSelectList = data;

            List<SelectListItem> CateNewsIDs = new List<SelectListItem>();
            foreach (CateNewsModel item in modelq.CateNewss)
            {

                SelectListItem item1 = new SelectListItem { Text = item.CateNewsName, Value = item.Id.ToString() };
                CateNewsIDs.Add(item1);
            }
            model.CateNewsIDs = CateNewsIDs;
            model.TransactionDateFrom = model.PostDate.ToString("dd/MM/yyyy");
            return View(model);
        }

        [Route("edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(ContentNewsModel faqModel)
        {
            if (ModelState.IsValid)
            {
                DateTime time = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(faqModel.TransactionDateFrom) && DateTime.TryParseExact(faqModel.TransactionDateFrom, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out time))
                {
                    faqModel.PostDate = time;

                    bool isValidType = false;

                    if (faqModel.NewsImage == null) isValidType = true;
                    if (faqModel.NewsImage?.Length > 0)
                    {
                        var name = faqModel.NewsImage.FileName;
                        string fileName = name.Split('\\').Last();
                        string extension = Path.GetExtension(fileName).ToLower();
                        if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif")
                            isValidType = true;
                    }

                    if (!isValidType)
                    {
                        ModelState.AddModelError("NewsImage", Model.Resources.ValidationMessages.WrongFileType);
                        CateNewssModel modelq = new CateNewssModel();
                        modelq.CateNewss = await _newsService.GetAllCateNews();
                        // ViewBag.CateNewsSelectList = data;

                        List<SelectListItem> CateNewsIDs = new List<SelectListItem>();
                        foreach (CateNewsModel item in modelq.CateNewss)
                        {

                            SelectListItem item1 = new SelectListItem { Text = item.CateNewsName, Value = item.Id.ToString() };
                            CateNewsIDs.Add(item1);
                        }
                        faqModel.CateNewsIDs = CateNewsIDs;
                    }
                    await _newsService.UpdateContentNews(faqModel);
                }
                var id = faqModel.Id;
                return RedirectToAction(nameof(ContentNewsController.List));
            }
            return View(faqModel);
        }
        #endregion Update Content News
        [Authorize(Policy = "AccountantInvestmentManagerAccess")]
        [Route("delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _newsService.DeleteContentNews(id);
            return RedirectToAction(nameof(ContentNewsController.List));
        }

       
        [Authorize(Policy = "AccountantInvestmentManagerAccess")]
        [Route("UploadImage")]

        [HttpPost]
        public ActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;
            bool isValidType = false;

       
            if (upload?.Length > 0)
            {
                var name = upload.FileName;
                string fileName1 = name.Split('\\').Last();
                string extension = Path.GetExtension(fileName1).ToLower();
                if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif")
                    isValidType = true;
            }

       
            if (!isValidType)
            {
                var NotImageMessage = "Vui lòng chọn một bức ảnh";
                dynamic NotImage = JsonConvert.DeserializeObject("{ 'uploaded': 0, 'error': { 'message': \"" + NotImageMessage + "\"}}");
                return Json(NotImage);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();

           

            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/images/news",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }

            var url = $"{"/images/news/"}{fileName}";
            var successMessage = "image is uploaded successfully";
            dynamic success = JsonConvert.DeserializeObject("{ 'uploaded': 1,'fileName': \"" + fileName + "\",'url': \"" + url + "\", 'error': { 'message': \"" + successMessage + "\"}}");
            return Json(new { uploaded = true, url });
        }
      
    }

}
