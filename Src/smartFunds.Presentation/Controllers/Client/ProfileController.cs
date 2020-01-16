using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using smartFunds.Common;
using smartFunds.Common.Helpers;
using smartFunds.Infrastructure.Models;
using smartFunds.Infrastructure.Services;
using smartFunds.Model.Client;
using smartFunds.Model.Common;
using smartFunds.Presentation.Models;
using smartFunds.Presentation.Models.Client;
using smartFunds.Service.Services;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using smartFunds.Common.Exceptions;
using static smartFunds.Common.Constants;
using smartFunds.Model.Resources;
using System.Globalization;

namespace smartFunds.Presentation.Controllers
{
    [Route("profile")]
    public class ProfileController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IUserService _userService;
        private readonly ITransactionHistoryService _transactionHistoryService;
        private readonly IInvestmentTargetService _investmentTargetService;
        private readonly IConfiguration _configuration;

        public ProfileController(ICustomerService customerService, IUserService userService, ITransactionHistoryService transactionHistoryService, IInvestmentTargetService investmentTargetService, IConfiguration configuration)
        {
            _customerService = customerService;
            _userService = userService;
            _transactionHistoryService = transactionHistoryService;
            _configuration = configuration;
            _investmentTargetService = investmentTargetService;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> Detail()
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await _userService.GetCurrentUser();

            DetailCustomerViewModel model = new DetailCustomerViewModel();
            model.Customer = currentUser;

            var dateFrom = DateTime.Now.AddDays(-30).Date;
            var dateTo = DateTime.Now.Date;
            model.PropertyFluctuations = await _transactionHistoryService.GetPropertyFluctuations(currentUser.Id, dateFrom, dateTo);

            model.InvestmentTarget = await _investmentTargetService.GetInvestmentTarget(currentUser.Id);

            return View(model);
        }

        [Route("PropertyFluctuations")]
        [HttpPost]
        public async Task<IActionResult> PropertyFluctuations(string dateFrom, string dateTo)
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await _userService.GetCurrentUser();

            var model = new PropertyFluctuations();

            DateTime from = new DateTime();
            DateTime to = new DateTime();
            if (string.IsNullOrWhiteSpace(dateFrom) || string.IsNullOrWhiteSpace(dateTo) || !DateTime.TryParseExact(dateFrom, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out from) || !DateTime.TryParseExact(dateTo, "dd/MM/yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out to) || from > to)
            {
                from = DateTime.Now.AddDays(-30).Date;
                to = DateTime.Now.Date;
            }

            model = await _transactionHistoryService.GetPropertyFluctuations(currentUser.Id, from, to);

            return Ok(model);
        }

        [Route("edit")]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await _userService.GetCurrentUser();

            var model = new EditCustomerViewModel();
            model.CustomerId = currentUser.Id;
            model.FullName = currentUser.FullName;
            model.PhoneNumber = currentUser.PhoneNumber;
            return View(model);
        }

        [Route("edit")]
        [HttpPost]
        public async Task<IActionResult> Edit(string FullName)
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            if(string.IsNullOrWhiteSpace(FullName))
            {
                return Json(new { Success = false, Message = ValidationMessages.EditCustomerError });
            }

            var currentUser = await _userService.GetCurrentUser();

            currentUser.FullName = FullName;

            var updateCustomer = await _customerService.UpdateCustomer(currentUser);
            if (updateCustomer)
            {
                return Json(new { Success = true, Message = "" });
            }

            return Json(new { Success = false, Message = ValidationMessages.EditCustomerError });
        }

        [HttpGet]
        [Route("change-password")]
        public IActionResult ChangePassword()
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View();
        }

        [HttpPost]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            var currentUser = await _userService.GetCurrentUser();
            var changedPassword = await _userService.ChangePassword(currentUser.UserName, model.CurrentPassword, model.NewPassword);
            return RedirectToAction(nameof(ProfileController.Detail), "Profile");
        }
    }
}
