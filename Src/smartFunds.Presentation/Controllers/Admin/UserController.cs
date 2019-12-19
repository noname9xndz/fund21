using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using smartFunds.Common;
using smartFunds.Common.Helpers;
using smartFunds.Model.Admin;
using smartFunds.Model.Common;
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Controllers.Admin
{
    [Route("admin/user")]
    [Authorize(Policy = "OnlyAdminAccess")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [Route("list")]
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var model = await _userService.GetAllAdminUsers();
            return View(model);
        }

        [Route("list")]
        [HttpPost]
        public async Task<IActionResult> List(int pageSize = 0, int pageIndex = 0)
        {
            var size = pageSize > 0 ? pageSize : _configuration.GetValue<int>("PagingConfig:PageSize");
            var page = pageIndex > 0 ? pageIndex : 1;

            var model = await _userService.GetAdminUsers(size, page);
            model.PageIndex = page;
            model.PageSize = size;

            return Ok(model);
        }

        [Route("new")]
        [HttpGet]
        public IActionResult New()
        {
            var model = new NewUserViewModel();
            return View(model);
        }

        [Route("new")]
        [HttpPost]
        public async Task<IActionResult> New(NewUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newUser = new UserModel() { FullName = model.FullName, UserName = model.Email, Email = model.Email, EmailConfirmed = true, PhoneNumberConfirmed = true };

            var addUser = await _userService.RegisterUser(newUser, model.Password, model.Role);

            if (addUser == RegisterStatus.Succeeded)
            {
                return RedirectToAction(nameof(UserController.List), "User");
            }
            else if (addUser == RegisterStatus.ExistUser)
            {
                ViewData["Message"] = Model.Resources.ValidationMessages.ExistEmail;
                return View(model);
            }

            ViewData["Message"] = Model.Resources.ValidationMessages.CreateError;
            return View(model);
        }

        [Route("delete")]
        public async Task<IActionResult> Delete(List<string> UserIds = null)
        {
            if (UserIds == null || !UserIds.Any())
            {
                TempData["Message"] = Model.Resources.ValidationMessages.UserNotSelect;
                return RedirectToAction(nameof(UserController.List), "User");
            }

            var deleteUsers = await _userService.DeleteUserByIds(UserIds);
            if (deleteUsers)
            {
                TempData["Message"] = Model.Resources.ValidationMessages.DeleteUserSuccess;
            }
            else
            {
                TempData["Message"] = Model.Resources.ValidationMessages.DeleteUserError;
            }

            return RedirectToAction(nameof(UserController.List), "User");
        }
    }
}
