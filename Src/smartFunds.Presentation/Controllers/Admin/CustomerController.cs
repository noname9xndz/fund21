using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smartFunds.Model.Common;
using smartFunds.Service.Services;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Controllers.Admin
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Authorize(Policy = "OnlyAdminAccess")]
        public async Task<IActionResult> Detail(string id)
        {
            UserModel model = await _customerService.GetCustomerById(id);
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "OnlyAdminAccess")]
        public async Task<IActionResult> List(int pageSize = 0, int pageIndex = 0)
        {
            var size = pageSize > 0 ? pageSize : 10;
            var page = pageIndex > 0 ? pageIndex : 1;

            CustomersModel model = new CustomersModel();
            model = await _customerService.GetListCustomer(size, page);
            return View(model);
        }

        //[HttpGet]
        //public IActionResult New()
        //{            
        //    return View();
        //}

        //[HttpPost]        
        //public async Task<IActionResult> Save(FAQModel faqModel)
        //{
        //    var savedFAQ = await _faqService.SaveFAQ(faqModel);
        //    return RedirectToAction("List");
        //}

    }
}
