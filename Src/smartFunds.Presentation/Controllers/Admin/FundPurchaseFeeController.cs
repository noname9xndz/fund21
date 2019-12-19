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
    [Route("admin/fund-purchase-fee")]
    public class FundPurchaseFeeController : Controller
    {
        private readonly IFundPurchaseFeeService _fundPurchaseFeeService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;

        public FundPurchaseFeeController(IFundPurchaseFeeService fundPurchaseFeeService, IConfiguration configuration, IPortfolioService portfolioService, IUserService userService, IEmailSender emailSender)
        {
            _fundPurchaseFeeService = fundPurchaseFeeService;
            _configuration = configuration;
            _userService = userService;
            _emailSender = emailSender;
        }

        [Route("add")]
        [HttpGet]
        public IActionResult Add(int fundId)
        {
            var model = new FundPurchaseFeeViewModel();
            model.FundId = fundId;
            return View(model);
        }

        [Route("add")]
        [HttpPost]
        public async Task<IActionResult> Add(FundPurchaseFeeViewModel fundPurchaseFeeView)
        {
            if (!ModelState.IsValid)
            {
                return View(fundPurchaseFeeView);
            }

            if(fundPurchaseFeeView.From < 0)
            {
                ViewData["Error"] = ValidationMessages.FromInvalid;
                return View(fundPurchaseFeeView);
            }

            if (fundPurchaseFeeView.To < -1 || fundPurchaseFeeView.To == 0)
            {
                ViewData["Error"] = ValidationMessages.ToInvalid;
                return View(fundPurchaseFeeView);
            }

            var model = new FundPurchaseFeeModel()
            {
                FundId = fundPurchaseFeeView.FundId,
                FromLabel = fundPurchaseFeeView.FromLabel,
                ToLabel = fundPurchaseFeeView.ToLabel,
                Fee = fundPurchaseFeeView.Fee
            };

            if(fundPurchaseFeeView.DecimalFromLabel == DecimalLabel.Million)
            {
                model.From = (decimal)fundPurchaseFeeView.From * 1000000;
            }
            else
            {
                model.From = (decimal)fundPurchaseFeeView.From * 1000000000;
            }

            if(fundPurchaseFeeView.To == -1)
            {
                model.To = -1;
            }
            else
            {
                if (fundPurchaseFeeView.DecimalToLabel == DecimalLabel.Million)
                {
                    model.To = (decimal)fundPurchaseFeeView.To * 1000000;
                }
                else
                {
                    model.To = (decimal)fundPurchaseFeeView.To * 1000000000;
                }
            }

            await _fundPurchaseFeeService.SaveFundPurchaseFee(model);
            return RedirectToAction(nameof(SettingsCMSController.Index), "SettingsCMS");
        }

        [Route("update")]
        [HttpGet]
        public async Task<IActionResult> Update(int fundPurchaseFeeId)
        {
            var fundPurchaseFee = await _fundPurchaseFeeService.GetFundPurchaseFee(fundPurchaseFeeId);
            if(fundPurchaseFee != null)
            {
                var model = new FundPurchaseFeeViewModel()
                {
                    Id = fundPurchaseFee.Id,
                    FundId = fundPurchaseFee.FundId,
                    FromLabel = fundPurchaseFee.FromLabel,
                    ToLabel = fundPurchaseFee.ToLabel,
                    Fee = fundPurchaseFee.Fee
                };

                if(fundPurchaseFee.From % 1000000000 == 0)
                {
                    model.From = (int)fundPurchaseFee.From / 1000000000;
                    model.DecimalFromLabel = DecimalLabel.Billion;
                }
                else
                {
                    model.From = (int)fundPurchaseFee.From / 1000000;
                    model.DecimalFromLabel = DecimalLabel.Million;
                }

                if(fundPurchaseFee.To == -1)
                {
                    model.To = -1;
                }
                else if (fundPurchaseFee.To % 1000000000 == 0)
                {
                    model.To = (int)fundPurchaseFee.To / 1000000000;
                    model.DecimalToLabel = DecimalLabel.Billion;
                }
                else
                {
                    model.To = (int)fundPurchaseFee.To / 1000000;
                    model.DecimalToLabel = DecimalLabel.Million;
                }

                return View(model);
            }

            return RedirectToAction(nameof(SettingsCMSController.Index), "SettingsCMS");
        }

        [Route("update")]
        [HttpPost]
        public async Task<IActionResult> Update(FundPurchaseFeeViewModel fundPurchaseFeeView)
        {
            if (!ModelState.IsValid)
            {
                return View(fundPurchaseFeeView);
            }

            if (fundPurchaseFeeView.From < 0)
            {
                ViewData["Error"] = ValidationMessages.FromInvalid;
                return View(fundPurchaseFeeView);
            }

            if (fundPurchaseFeeView.To < -1 || fundPurchaseFeeView.To == 0)
            {
                ViewData["Error"] = ValidationMessages.ToInvalid;
                return View(fundPurchaseFeeView);
            }

            var model = new FundPurchaseFeeModel()
            {
                Id = fundPurchaseFeeView.Id,
                FundId = fundPurchaseFeeView.FundId,
                FromLabel = fundPurchaseFeeView.FromLabel,
                ToLabel = fundPurchaseFeeView.ToLabel,
                Fee = fundPurchaseFeeView.Fee
            };

            if (fundPurchaseFeeView.DecimalFromLabel == DecimalLabel.Million)
            {
                model.From = (decimal)fundPurchaseFeeView.From * 1000000;
            }
            else
            {
                model.From = (decimal)fundPurchaseFeeView.From * 1000000000;
            }

            if (fundPurchaseFeeView.To == -1)
            {
                model.To = -1;
            }
            else
            {
                if (fundPurchaseFeeView.DecimalToLabel == DecimalLabel.Million)
                {
                    model.To = (decimal)fundPurchaseFeeView.To * 1000000;
                }
                else
                {
                    model.To = (decimal)fundPurchaseFeeView.To * 1000000000;
                }
            }

            await _fundPurchaseFeeService.UpdateFundPurchaseFee(model);
            return RedirectToAction(nameof(SettingsCMSController.Index), "SettingsCMS");
        }

        [Route("delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(List<int> fundPurchaseFeeIds = null)
        {
            if (fundPurchaseFeeIds == null)
            {
                return RedirectToAction(nameof(SettingsCMSController.Index), "SettingsCMS");
            }

            await _fundPurchaseFeeService.DeleteListFundPurchaseFee(fundPurchaseFeeIds);

            return RedirectToAction(nameof(SettingsCMSController.Index), "SettingsCMS");
        }
    }
}
