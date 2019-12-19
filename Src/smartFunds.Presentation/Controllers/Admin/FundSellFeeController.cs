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
using smartFunds.Presentation.Controllers.Admin;
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers
{
    [Authorize(Policy = "OnlyAdminAccess")]
    [Route("admin/fund-sell-fee")]
    public class FundSellFeeController : Controller
    {
        private readonly IFundSellFeeService _fundSellFeeService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;

        public FundSellFeeController(IFundSellFeeService fundSellFeeService, IConfiguration configuration, IPortfolioService portfolioService, IUserService userService, IEmailSender emailSender)
        {
            _fundSellFeeService = fundSellFeeService;
            _configuration = configuration;
            _userService = userService;
            _emailSender = emailSender;
        }

        [Route("add")]
        [HttpGet]
        public IActionResult Add(int fundId)
        {
            var model = new FundSellFeeViewModel();
            model.FundId = fundId;
            return View(model);
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> Add(FundSellFeeViewModel fundSellFeeView)
        {
            if (!ModelState.IsValid)
            {
                return View(fundSellFeeView);
            }

            if(fundSellFeeView.From < 0)
            {
                ViewData["Error"] = ValidationMessages.FromInvalid;
                return View(fundSellFeeView);
            }

            if (fundSellFeeView.To < -1 || fundSellFeeView.To == 0)
            {
                ViewData["Error"] = ValidationMessages.ToInvalid;
                return View(fundSellFeeView);
            }

            var model = new FundSellFeeModel()
            {
                FundId = fundSellFeeView.FundId,
                FromLabel = fundSellFeeView.FromLabel,
                ToLabel = fundSellFeeView.ToLabel,
                Fee = fundSellFeeView.Fee
            };

            if(fundSellFeeView.DateFromLabel == DateLabel.Month)
            {
                model.From = fundSellFeeView.From * 30;
            }
            else if (fundSellFeeView.DateFromLabel == DateLabel.Year)
            {
                model.From = fundSellFeeView.From * 365;
            }
            else
            {
                model.From = fundSellFeeView.From;
            }

            if(fundSellFeeView.To == -1)
            {
                model.To = -1;
            }
            else
            {
                if (fundSellFeeView.DateToLabel == DateLabel.Month)
                {
                    model.To = fundSellFeeView.To * 30;
                }
                else if (fundSellFeeView.DateToLabel == DateLabel.Year)
                {
                    model.To = fundSellFeeView.To * 365;
                }
                else
                {
                    model.To = fundSellFeeView.To;
                }
            }

            await _fundSellFeeService.SaveFundSellFee(model);
            return RedirectToAction(nameof(SettingsCMSController.Index), "SettingsCMS");
        }

        [Route("update")]
        [HttpGet]
        public async Task<IActionResult> Update(int fundSellFeeId)
        {
            var fundSellFee = await _fundSellFeeService.GetFundSellFee(fundSellFeeId);
            if(fundSellFee != null)
            {
                var model = new FundSellFeeViewModel()
                {
                    Id = fundSellFee.Id,
                    FundId = fundSellFee.FundId,
                    FromLabel = fundSellFee.FromLabel,
                    ToLabel = fundSellFee.ToLabel,
                    Fee = fundSellFee.Fee
                };

                if(fundSellFee.From % 365 == 0)
                {
                    model.From = fundSellFee.From / 365;
                    model.DateFromLabel = DateLabel.Year;
                }
                else if (fundSellFee.From % 30 == 0)
                {
                    model.From = fundSellFee.From / 30;
                    model.DateFromLabel = DateLabel.Month;
                }
                else
                {
                    model.From = fundSellFee.From;
                    model.DateFromLabel = DateLabel.Day;
                }

                if(fundSellFee.To == -1)
                {
                    model.To = -1;
                }
                else if (fundSellFee.To % 365 == 0)
                {
                    model.To = fundSellFee.To / 365;
                    model.DateToLabel = DateLabel.Year;
                }
                else if (fundSellFee.To % 30 == 0)
                {
                    model.To = fundSellFee.To / 30;
                    model.DateToLabel = DateLabel.Month;
                }
                else
                {
                    model.To = fundSellFee.To;
                    model.DateToLabel = DateLabel.Day;
                }

                return View(model);
            }

            return RedirectToAction(nameof(SettingsCMSController.Index), "SettingsCMS");
        }

        [Route("update")]
        [HttpPost]
        public async Task<IActionResult> Update(FundSellFeeViewModel fundSellFeeView)
        {
            if (!ModelState.IsValid)
            {
                return View(fundSellFeeView);
            }

            if (fundSellFeeView.From < 0)
            {
                ViewData["Error"] = ValidationMessages.FromInvalid;
                return View(fundSellFeeView);
            }

            if (fundSellFeeView.To < -1 || fundSellFeeView.To == 0)
            {
                ViewData["Error"] = ValidationMessages.ToInvalid;
                return View(fundSellFeeView);
            }

            var model = new FundSellFeeModel()
            {
                Id = fundSellFeeView.Id,
                FundId = fundSellFeeView.FundId,
                FromLabel = fundSellFeeView.FromLabel,
                ToLabel = fundSellFeeView.ToLabel,
                Fee = fundSellFeeView.Fee
            };

            if (fundSellFeeView.DateFromLabel == DateLabel.Month)
            {
                model.From = fundSellFeeView.From * 30;
            }
            else if (fundSellFeeView.DateFromLabel == DateLabel.Year)
            {
                model.From = fundSellFeeView.From * 365;
            }
            else
            {
                model.From = fundSellFeeView.From;
            }

            if (fundSellFeeView.To == -1)
            {
                model.To = -1;
            }
            else
            {
                if (fundSellFeeView.DateToLabel == DateLabel.Month)
                {
                    model.To = fundSellFeeView.To * 30;
                }
                else if (fundSellFeeView.DateToLabel == DateLabel.Year)
                {
                    model.To = fundSellFeeView.To * 365;
                }
                else
                {
                    model.To = fundSellFeeView.To;
                }
            }

            await _fundSellFeeService.UpdateFundSellFee(model);
            return RedirectToAction(nameof(SettingsCMSController.Index), "SettingsCMS");
        }

        [Route("delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(List<int> fundSellFeeIds = null)
        {
            if (fundSellFeeIds == null)
            {
                return RedirectToAction(nameof(SettingsCMSController.Index), "SettingsCMS");
            }

            await _fundSellFeeService.DeleteListFundSellFee(fundSellFeeIds);

            return RedirectToAction(nameof(SettingsCMSController.Index), "SettingsCMS");
        }
    }
}
