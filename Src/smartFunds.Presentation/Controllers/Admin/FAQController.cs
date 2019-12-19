using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using smartFunds.Model.Common;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers.Admin
{
    public class FAQController : Controller
    {
        private readonly IFAQService _faqService;

        public FAQController(IFAQService faqService)
        {
            _faqService = faqService;
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {            
            FAQModel model = await _faqService.GetFAQ(id);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> List(int? pageSize, int? pageIndex)
        {
            FAQsModel model = new FAQsModel();
            return View(model);
        }

        #region Add FAQ
        [HttpGet]
        public IActionResult New()
        {            
            return View();
        }

        [HttpPost]        
        public async Task<IActionResult> Save(FAQModel faqModel)
        {
            var savedFAQ = await _faqService.SaveFAQ(faqModel);
            return RedirectToAction("List");
        }
        #endregion Add FAQ

        #region Update FAQ
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            FAQModel model = await _faqService.GetFAQ(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(FAQModel faqModel)
        {
            _faqService.UpdateFAQ(faqModel);
            return RedirectToAction("Detail/" + faqModel.Id);
        }
        #endregion Update FAQ
    }
}
