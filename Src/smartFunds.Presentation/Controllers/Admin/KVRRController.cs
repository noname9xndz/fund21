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
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers.Admin
{
    [Route("admin/kvrr")]
    public class KVRRController : Controller
    {
        private readonly IKVRRService _kvrrService;
        private readonly IPortfolioService _portfolioService;

        public KVRRController(IKVRRService kvrrService, IPortfolioService portfolioService)
        {
            _kvrrService = kvrrService;
            _portfolioService = portfolioService;
        }

        [Authorize(Policy = "AdminManagerAccess")]
        [HttpGet]
        [Route("list")]
        public IActionResult List(int pageSize = 0, int pageIndex = 0)
        {
            KVRRsModel model = new KVRRsModel();
            //if (pageSize == 0)
            //{
            //    pageSize = _configuration.GetValue<int>("PagingConfig:PageSize");
            //}
            //if (pageIndex == 0)
            //{
            //    pageIndex = 1;
            //}
            model.KVRRs = _kvrrService.GetKVRRs(pageSize, pageIndex)?.ToList();
            return View("List", model);
        }

        [Authorize(Policy = "AdminManagerAccess")]
        [HttpGet]
        [Route("detail/{id}")]
        public IActionResult Detail(int id)
        {
            try
            {
                var kvrr = _kvrrService.GetKVRRById(id);
                return View(kvrr);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        #region Add New
        [Authorize(Policy = "CustomerManagerNotAccess")]
        [HttpGet]
        [Route("new")]
        public async Task<IActionResult> New()
        {
            var data = await _portfolioService.GetPortfoliosUnUse();
            ViewBag.PortfolioSelectList = data;
            return View(new KVRRModel());
        }

        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> New(KVRRModel kvrr)
        {
            if (kvrr == null) return Json(false, new JsonSerializerSettings());

            bool isValidType = false;

            if (kvrr.KVRRImage == null) isValidType = true;
            if (kvrr.KVRRImage?.Length > 0)
            {
                var name = kvrr.KVRRImage.FileName;
                string fileName = name.Split('\\').Last();
                string extension = Path.GetExtension(fileName).ToLower();
                if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif")
                    isValidType = true;
            }

            if (!isValidType)
            {
                ModelState.AddModelError("KVRRImage", Model.Resources.ValidationMessages.WrongFileType);
                var data = await _portfolioService.GetPortfoliosUnUse();
                ViewBag.PortfolioSelectList = data;
                return View(kvrr);
            }

            var result = await _kvrrService.Save(kvrr);

            return RedirectToAction("List");
        }
        #endregion

        #region Update
        [Authorize(Policy = "CustomerManagerNotAccess")]
        [HttpGet]
        [Route("edit")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var kvrr = _kvrrService.GetKVRRById(id);
                var data = await _portfolioService.GetPortfoliosUnUse();
                // 1 kvrr - multi Portfolio
                if (kvrr?.KVRRPortfolios != null && kvrr.KVRRPortfolios.Any())
                {
                    var listPortfolio = data?.ToList() ?? null;
                    foreach (var po in kvrr.KVRRPortfolios)
                    {
                        kvrr.PortfolioIds.Add(po.PortfolioId.ToString());
                    }

                    // 1 kvrr - 1 Portfolio
                    kvrr.PortfolioId = kvrr?.KVRRPortfolios.First()?.PortfolioId.ToString();
                    var currentPortfolio = kvrr?.KVRRPortfolios.First()?.Portfolio ?? null;
                    if (currentPortfolio != null && listPortfolio != null)
                    {
                        listPortfolio.Insert(0, currentPortfolio);
                        ViewBag.PortfolioSelectList = listPortfolio;
                    }
                }

                return View(kvrr);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit(KVRRModel kvrr)
        {
            try
            {
                if (kvrr == null) return Json(false, new JsonSerializerSettings());

                bool isValidType = false;

                if (kvrr.KVRRImage == null) isValidType = true;
                if (kvrr.KVRRImage?.Length > 0)
                {
                    string fileName = kvrr.KVRRImage.FileName;
                    string extension = Path.GetExtension(fileName).ToLower();
                    if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif")
                    {
                        isValidType = true;
                    }
                }

                if (!isValidType)
                {
                    ModelState.AddModelError("KVRRImage", Model.Resources.ValidationMessages.WrongFileType);

                    var data = await _portfolioService.GetPortfoliosUnUse();
                    var currentPortfolio = !string.IsNullOrEmpty(kvrr.PortfolioId)
                        ? _portfolioService.GetPortfolioById(Int32.Parse(kvrr.PortfolioId))
                        : null;

                    var listPortfolio = new List<PortfolioModel> { currentPortfolio };
                    listPortfolio.AddRange(data);
                    ViewBag.PortfolioSelectList = listPortfolio;

                    return View(kvrr);
                }

                await _kvrrService.Update(kvrr);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("List");
        }

        #endregion

        [HttpPost("IsDuplicateName")]
        public async Task<IActionResult> IsDuplicateName(string Name, string initName)
        {
            return Json(await _kvrrService.IsDuplicateName(Name, initName));
        }
    }
}
