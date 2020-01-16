using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using smartFunds.Common;
using smartFunds.Infrastructure.Models;
using smartFunds.Infrastructure.Services;
using smartFunds.Model.Common;
using smartFunds.Model.Resources;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers.Client
{
    [Route("contact")]
    public class ContactController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly IContactCMSService _contactCMSService;
        private readonly IConfiguration _configuration;
        public ContactController(IEmailSender emailSender, IConfiguration configuration, IContactCMSService contactCMSService)
        {
            _emailSender = emailSender;
            _configuration = configuration;
            _contactCMSService = contactCMSService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Contact()
        {
            try
            {
                //get contact from db
                var _contact = _contactCMSService.GetContactConfiguration();
                ViewBag.Email = _contact.Email;
                ViewBag.Hotline = _contact.Phone;
                var contact = new ContactModel();
                return View("~/Views/Support/Contact.cshtml", contact);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("~/Views/Support/Contact.cshtml", model);
                }

                var _contact = _contactCMSService.GetContactConfiguration();
                var contentEmail = string.Empty;
                contentEmail += "Người gửi: " + model.FullName;
                contentEmail += "<br />Số điện thoại: " + model.PhoneNumber;
                contentEmail += "<br />Email: " + model.Email;
                contentEmail += "<br />Nội dung: " + model.Content;
                var mailConfig = SetMailConfig(model.Email, _contact.EmailForReceiving, model.FullName, _configuration.GetValue<string>("EmailSubject:Contact"), contentEmail);
                var sendEmail = _emailSender.SendEmail(mailConfig);
                if (sendEmail)
                    ViewData["Message"] = ValidationMessages.SendMailSucess;
                else
                    ViewData["Message"] = ValidationMessages.SendMailError;

                return RedirectToAction("Contact");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private MailConfig SetMailConfig(string from, string to, string senderName, string subject, string body)
        {
            return new MailConfig()
            {
                EmailFrom = from,
                EnableSsl = _configuration.GetValue<bool>("EmailConfig:EnableSsl"),
                Port = _configuration.GetValue<int>("EmailConfig:Port"),
                Host = _configuration.GetValue<string>("EmailConfig:Host"),
                Username = _configuration.GetValue<string>("EmailConfig:Username"),
                Password = _configuration.GetValue<string>("EmailConfig:Password"),
                EmailSenderName = senderName,
                EmailSubject = subject,
                MailTo = string.IsNullOrEmpty(to) ? _configuration.GetValue<string>("EmailConfig:EmailFrom"): to,
                EmailBody = body
            };
        }
    }
}
