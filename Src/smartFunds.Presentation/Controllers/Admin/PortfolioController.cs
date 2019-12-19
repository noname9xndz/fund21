using System;
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
using smartFunds.Data.Repositories;
using smartFunds.Infrastructure.Models;
using smartFunds.Infrastructure.Services;
using smartFunds.Model.Common;
using smartFunds.Model.Resources;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers.Admin
{
    [Authorize(Policy = "AdminManagerAccess")]
    [Route("admin/portfolio")]
    public class PortfolioController : Controller
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IFundService _fundService;
        private readonly IHomepageCMSService _homepageCMSService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;

        public PortfolioController(IPortfolioService portfolioService, IFundService fundService, IHomepageCMSService homepageCMSService, IConfiguration configuration, IUserService userService, IEmailSender emailSender)
        {
            _portfolioService = portfolioService;
            _homepageCMSService = homepageCMSService;
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
                PortfoliosModel model = new PortfoliosModel();
                //pageSize = pageSize == 0 ? _configuration.GetValue<int>("PagingConfig:PageSize") : pageSize;
                //pageIndex = pageIndex == 0 ? 1 : pageIndex;

                ViewBag.SearchPortfoliosModel = new SearchPortfolio();
                model.Portfolios = _portfolioService.GetPortfolios(pageSize, pageIndex)?.ToList();

                return View(model);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Policy = "OnlyAccountantAccess")]
        [HttpGet]
        [Route("new")]
        public IActionResult New()
        {
            try
            {
                return View(new PortfolioModel());
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("update")]
        public ActionResult UpdateImage()
        {
            HomepageCMSModel cmsModel = new HomepageCMSModel();
            cmsModel = _homepageCMSService.GetAll().First(x => x.Category == "2");
            return View(cmsModel);
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> UpdateImage(IFormFile image, string typeUpload = "", int Id = 0)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isValidType = false;
                    if (image?.Length > 0)
                    {
                        string fileName = image.FileName;
                        string extension = Path.GetExtension(fileName).ToLower();
                        if (extension == ".png" || extension == ".jpg" || extension == ".jpeg" || extension == ".gif")
                            isValidType = true;
                        if (!isValidType)
                        {
                            ModelState.AddModelError("Banner", Model.Resources.ValidationMessages.WrongFileType);
                            return Json(false, new JsonSerializerSettings());
                        }
                        HomepageCMSModel homepageCMS = new HomepageCMSModel();
                        //homepageCMS.ImageName = "mobile_"+fileName;
                        homepageCMS.Banner = image;
                        homepageCMS.Category = "2";
                        await _portfolioService.UpdatePortfolioImage(homepageCMS, typeUpload, Id);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Index", "SettingsCMS");
        }

        [HttpPost]
        [Route("new")]
        public async Task<IActionResult> New(PortfolioModel portfolio)
        {
            try
            {
                if (portfolio == null) return Json(false, new JsonSerializerSettings());
                if (!ModelState.IsValid)
                {
                    return View(portfolio);
                }

                var result = await _portfolioService.Save(portfolio);
                return RedirectToAction("Detail", new { id = result.Id });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Policy = "OnlyAccountantAccess")]
        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            try
            {
                var portfolio = _portfolioService.GetPortfolioById(id);
                return View(portfolio);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("editportfolio")]
        public async Task<IActionResult> EditPortfolio(PortfolioModel portfolio)
        {
            try
            {
                if (portfolio == null) return Json(false, new JsonSerializerSettings());
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Edit", new { id = portfolio.Id });
                }
                var result = await _portfolioService.Update(portfolio);
                return RedirectToAction("Detail", new { id = portfolio.Id });
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("detail/{id}")]
        public IActionResult Detail(int id)
        {
            try
            {
                var portfolio = _portfolioService.GetPortfolioById(id);
                if (portfolio == null) throw new NotFoundException();

                var funds = _fundService.GetAllFund();

                if (funds != null && funds.Any())
                {
                    foreach (var fund in funds)
                    {
                        var portfolioFundIndex = portfolio.PortfolioFunds.FindIndex(x => x.FundId == fund.Id);
                        if (portfolioFundIndex < 0)
                        {
                            portfolio.PortfolioFunds.Add(new PortfolioFundModel
                            {
                                PortfolioId = portfolio.Id,
                                FundId = fund.Id,
                                Fund = fund
                            });
                        }
                    }
                }

                portfolio.PortfolioFunds.OrderBy(x => x.FundId).ToList();
                return View(portfolio);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Policy = "CustomerManagerNotAccess")]
        [HttpPost]
        [Route("export")]
        public IActionResult Export(SearchPortfolio searchPortfolio)
        {
            try
            {
                var fileContent = _portfolioService.ExportPortfolio(searchPortfolio);
                return File(fileContent?.Result, "application/ms-excel", $"Portfolios.xlsx");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        [Authorize(Policy = "OnlyAccountantAccess")]
        [HttpPost]
        [Route("savefunds")]
        public async Task<IActionResult> SaveFunds(PortfolioModel portfolioModel)
        {
            if (portfolioModel.PortfolioFunds == null || !portfolioModel.PortfolioFunds.Any()) throw new InvalidParameterException();

            var percentList = portfolioModel.PortfolioFunds.Where(x => x.FundPercentNew != null && x.FundPercentNew.Value > 0).Select(x => x.FundPercentNew.Value).ToList();
            decimal sumPercent = 0;
            foreach (var percent in percentList)
            {
                sumPercent += percent;
            }

            if (sumPercent != 100)
            {
                ModelState.AddModelError("IsFundPercent100", Model.Resources.ValidationMessages.FundPercent100);
                portfolioModel.KVRRPortfolios = _portfolioService.GetPortfolioById(portfolioModel.Id)?.KVRRPortfolios;
                return View("Detail", portfolioModel);
            }
            var portfolioFundOld = _portfolioService.GetPortfolioFundByPortfolioId(portfolioModel.Id).Result;

            await _portfolioService.SaveFunds(portfolioModel);

            if (CheckListPortfolioFund(portfolioModel.PortfolioFunds, portfolioFundOld))
            {
                // Send mail
                var listAdmins = _userService.GetAdminUsers().Result;

                if (listAdmins != null && listAdmins.Count > 0)
                {
                    foreach (var admin in listAdmins)
                    {
                        if (!string.IsNullOrEmpty(admin.Email))
                        {
                            var sender = _userService.GetCurrentUser().Result;
                            var contentEmail = _configuration.GetValue<string>("EmailBody:RequestApproved").Replace("[FullName]", admin.FullName).Replace("[Sender]", sender.FullName).Replace("[SenderEmail]", sender.Email).Replace("[TaskType]", Model.Resources.Common.RequestPortfolioFund);

                            var mailConfig = SetMailConfig(sender.Email, admin.Email, _configuration.GetValue<string>("EmailSubject:RequestApproved"), contentEmail);
                            var sendEmail = _emailSender.SendEmail(mailConfig);
                            if (sendEmail)
                                ViewData["Message"] = ValidationMessages.SendMailSucess;
                            else
                                ViewData["Message"] = ValidationMessages.SendMailError;
                        }
                    }
                }
            }
            return RedirectToAction("Detail", new { id = portfolioModel.Id });
        }

        private bool CheckListPortfolioFund(List<PortfolioFundModel> listNew, List<PortfolioFundModel> listOld)
        {
            var check = false;
            foreach (var item in listNew)
            {
                var oldItem = listOld.FirstOrDefault(x => x.FundId == item.FundId && x.PortfolioId == item.PortfolioId);
                if(oldItem == null)
                {
                    if(item.FundPercentNew != null)
                        return true;
                }
                else
                {
                    item.FundPercentNew = item.FundPercentNew == null ? 0 : item.FundPercentNew;
                    if (oldItem.FundPercentNew != item.FundPercentNew)
                        return true;
                }
            }
            return check;
        }

        [HttpPost("IsPortfolioNameExists")]
        public async Task<IActionResult> IsPortfolioNameExists(string Title, string initTitle)
        {
            return Json(await _portfolioService.IsPortfolioNameExists(Title, initTitle));
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
