using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using smartFunds.Common;
using smartFunds.Common.Helpers;
using smartFunds.Infrastructure.Services;
using smartFunds.Model.Admin;
using smartFunds.Model.Common;
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Controllers.Admin
{
    [Authorize(Policy = "OnlyAdminAccess")]
    [Route("admin/push-notification")]
    public class PushNotificationController : Controller
    {
        private readonly IPushNotification _pushNotification;
        private readonly IConfiguration _configuration;

        public PushNotificationController(IPushNotification pushNotification, IConfiguration configuration)
        {
            _pushNotification = pushNotification;
            _configuration = configuration;
        }

        [Route("")]
        [HttpGet]
        public IActionResult SendNotification()
        {
            return View();
        }

        [Route("")]
        [HttpPost]
        public IActionResult SendNotification(PushNotificationViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var pushNotification = _pushNotification.Send(model.Title, model.Content, _configuration.GetValue<string>("PushNotification:AppId"), _configuration.GetValue<string>("PushNotification:RestApiKey"));
            if(pushNotification)
            {
                ViewData["SuccessMessage"] = Model.Resources.Common.PushNotificationSuccess;
            }
            else
            {
                ViewData["ErrorMessage"] = Model.Resources.Common.PushNotificationError;
            }
            return View(model);
        }
    }
}
