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
    [Authorize(Policy = "AdminManagerAccess")]
    [Route("admin/contactcms")]
    public class ContactCMSController : Controller
    {
        private readonly IContactCMSService _contactConfigurationService;
        public ContactCMSController(IContactCMSService contactConfigurationService)
        {
            _contactConfigurationService = contactConfigurationService;
        }

        [HttpGet]
        [Route("displaycontactcms")]
        public async Task<IActionResult> DisplayContactCMS()
        {
            var data = _contactConfigurationService.GetContactConfiguration();
            if (data == null)
            {
                //default value
                data = new ContactCMSModel
                {
                    Email = "smartfunds@gmail.com",
                    Phone = "0123456789",
                    EmailForReceiving = "abc_smartfunds@gmail.com",
                };
                try
                {
                    await Add(data);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return View(data);
        }

        [Route("AddNewOne")]
        [HttpPost]
        public async Task<IActionResult> Add(ContactCMSModel model)
        {

            await _contactConfigurationService.AddDefault(model);
            return RedirectToAction(nameof(ContactCMSController.DisplayContactCMS));
        }

        [HttpGet]
        [Route("updatecontactcms")]
        public IActionResult UpdateContactCMS()
        {
            var data = _contactConfigurationService.GetContactConfiguration();
            return View(data);
        }

        [HttpPost]
        [Route("updatecontactcms")]
        public async Task<IActionResult> UpdateContactCMS(ContactCMSModel contactCMSModel)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            await _contactConfigurationService.UpdateContactCMS(contactCMSModel);
            return RedirectToAction("Index", "SettingsCMS");
        }
    }
}
