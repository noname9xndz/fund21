using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using smartFunds.Common;
using smartFunds.Model.Common;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers.Client
{
    [Route("faqs")]
    public class FAQController : Controller
    {
        private readonly IFAQService _faqService;

        public FAQController(IFAQService faqService)
        {
            _faqService = faqService;
        }

        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> FAQs(FAQCategory category = FAQCategory.All, string key = "")
        {
            try
            {
                FAQsModel model = new FAQsModel();
                model.FAQs = await _faqService.GetFAQsByCategory(category, key);
                ViewBag.SetActive = category.ToString();
                ViewBag.ValueSearch = key;
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}