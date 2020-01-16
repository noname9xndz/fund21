using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using smartFunds.Common;
using smartFunds.Common.Exceptions;
using smartFunds.Common.Helpers;
using smartFunds.Infrastructure.Models;
using smartFunds.Infrastructure.Services;
using smartFunds.Model.Common;
using smartFunds.Model.Resources;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers
{
    [Route("admin/fund")]
    public class FundController : Controller
    {
        private readonly IFundService _fundService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;

        public FundController(IFundService fundService, IConfiguration configuration, IPortfolioService portfolioService, IUserService userService, IEmailSender emailSender)
        {
            _fundService = fundService;
            _configuration = configuration;
            _userService = userService;
            _emailSender = emailSender;
        }

        [Authorize(Policy = "AdminManagerAccess")]
        [Route("list")]
        public IActionResult List(int pageSize = 0, int pageIndex = 0)
        {
            try
            {
                FundsModel model = new FundsModel();
                //pageSize = pageSize == 0 ? _configuration.GetValue<int>("PagingConfig:PageSize") : pageSize;
                //pageIndex = pageIndex == 0 ? 1 : pageIndex;

                ViewBag.SearchFundsModel = new SearchFund();

                model.Funds = _fundService.GetFunds(pageSize, pageIndex)?.ToList();

                return View(model);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Policy = "AdminManagerAccess")]
        [Route("list")]
        [HttpPost]
        public async Task<IActionResult> List([FromBody]ListUpdateNav data)
        {
            try
            {
                var listFundsUpdateNav = new List<FundModel>();
                foreach (var _fund in data.UpdateNav)
                {
                    if (_fund.NAVNew > 0)
                    {
                       listFundsUpdateNav.Add(new FundModel() { Id = _fund.ID, NAVNew = _fund.NAVNew });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Giá trị NAV mới phải > 0" });
                    }
                }
                if (listFundsUpdateNav.Count <= 0) return RedirectToAction("List");
                var result = await _fundService.Updates(listFundsUpdateNav);
                // Send mail
                var listAdmins = await _userService.GetAdminUsers();

                if (listAdmins != null && listAdmins.Count > 0)
                {
                    foreach (var admin in listAdmins)
                    {
                        if (!string.IsNullOrEmpty(admin.Email))
                        {
                            var sender = await _userService.GetCurrentUser();
                            var contentEmail = _configuration.GetValue<string>("EmailBody:RequestApproved").Replace("[FullName]", admin.FullName).Replace("[Sender]", sender.FullName).Replace("[SenderEmail]", sender.Email).Replace("[TaskType]", Model.Resources.Common.RequestNav);

                            var mailConfig = SetMailConfig(sender.Email, admin.Email, _configuration.GetValue<string>("EmailSubject:RequestApproved"), contentEmail);
                            var sendEmail = _emailSender.SendEmail(mailConfig);
                            if (sendEmail)
                                ViewData["Message"] = ValidationMessages.SendMailSucess;
                            else
                                ViewData["Message"] = ValidationMessages.SendMailError;
                        }
                    }
                }
                return Json(new { success = true });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Policy = "OnlyAccountantAccess")]
        [Route("new")]
        [HttpGet]
        public IActionResult New()
        {
            try
            {
                return View(new FundModel());
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Route("new")]
        [HttpPost]
        public async Task<IActionResult> New(FundModel fund)
        {
            try
            {
                if (fund == null) return Json(false, new JsonSerializerSettings());
                if (!ModelState.IsValid)
                {
                    return View(fund);
                }
                if(fund.NAV <= 0)
                {
                    ModelState.AddModelError("NAV", Model.Resources.ValidationMessages.MinNAV);
                    return View(fund);
                }
                var result = await _fundService.Save(fund);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("List");
        }

        [Authorize(Policy = "OnlyAccountantAccess")]
        [Route("edit/{id}")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var fund = _fundService.GetFundById(id);
                return View(fund);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Route("editfund")]
        [HttpPost]
        public async Task<IActionResult> EditFund(FundModel fund)
        {
            try
            {
                if (fund == null) return Json(false, new JsonSerializerSettings());
                if (!ModelState.IsValid)
                {
                    return View("Edit", fund);
                }
                var result = await _fundService.Update(fund);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("List");
        }

        [Authorize(Policy = "AdminManagerAccess")]
        [Route("detail/{id}")]
        [HttpGet]
        public IActionResult Detail(int id)
        {
            try
            {
                var fund = _fundService.GetFundById(id);
                return View(fund);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Policy = "CustomerManagerNotAccess")]
        [Route("export")]
        public IActionResult Export(SearchFund searchFund)
        {
            try
            {
                var fileContent = _fundService.ExportFund(searchFund);
                return File(fileContent?.Result, "application/ms-excel", $"Funds.xlsx");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Policy = "OnlyAccountantAccess")]
        [Route("delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(IFormCollection collection)
        {
            List<int> fundIds = new List<int>();
            foreach (var key in collection.Keys)
            {
                if (key.Contains("checkbox_fund"))
                {
                    fundIds.Add(int.Parse(key.Replace("checkbox_fund", "")));
                }
            }

            await _fundService.DeleteFunds(fundIds.ToArray());
            return RedirectToAction("List");
        }

        [HttpPost("IsDuplicateName")]
        public async Task<IActionResult> IsDuplicateName(string Title, string initName)
        {
            return Json(await _fundService.IsDuplicateName(Title, initName));
        }

        [HttpPost("IsDuplicateCode")]
        public async Task<IActionResult> IsDuplicateCode(string Code, string initCode)
        {
            return Json(await _fundService.IsDuplicateCode(Code, initCode));
        }

        private MailConfig SetMailConfig(string from, string to, string subject, string body)
        {
            return new MailConfig()
            {
                EmailFrom = from,
                EnableSsl = _configuration.GetValue<bool>("EmailConfig:EnableSsl"),
                Port = _configuration.GetValue<int>("EmailConfig:Port"),
                Host = _configuration.GetValue<string>("EmailConfig:Host"),
                Username = _configuration.GetValue<string>("EmailConfig:Username"),
                Password = _configuration.GetValue<string>("EmailConfig:Password"),
                EmailSenderName = from,
                EmailSubject = subject,
                MailTo = to,
                EmailBody = body
            };
        }
    }
}
