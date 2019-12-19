using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using smartFunds.Common;
using smartFunds.Infrastructure.Models;
using smartFunds.Infrastructure.Services;
using smartFunds.Model.Common;
using smartFunds.Model.Resources;
using smartFunds.Presentation.Controllers.Admin;
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;
using System;
using System.Threading.Tasks;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public AdminController(IUserService userService, IConfiguration configuration, IEmailSender emailSender)
        {
            _userService = userService;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        [HttpGet]
        [Authorize(Policy = "AdminManagerAccess")]
        [Route("")]
        public async Task<IActionResult> Task()
        {
            var currentUser = await _userService.GetCurrentUser();
            if (_userService.IsInRole(currentUser, RoleName.Admin).Result)
            {
                return RedirectToAction(nameof(TaskController.ApproveList), "Task");
            }
            else if (_userService.IsInRole(currentUser, RoleName.Accountant).Result)
            {
                return RedirectToAction(nameof(TaskController.List), "Task");
            }
            else
            {
                return RedirectToAction(nameof(CustomerController.List), "Customer");
            }
        }

        [HttpGet]
        [Authorize(Policy = "AdminManagerAccess")]
        [Route("profile")]
        public async Task<IActionResult> Profile()
        {
            var currentUser = await _userService.GetCurrentUser();
            return View(currentUser);
        }

        [HttpGet]
        [Authorize(Policy = "AdminManagerAccess")]
        [Route("profile/update")]
        public async Task<IActionResult> UpdateProfile()
        {
            var currentUser = await _userService.GetCurrentUser();
            var model = new UpdateProfileViewModel();
            model.FullName = currentUser.FullName;
            model.Email = currentUser.Email;
            model.RoleName = _userService.GetRoleName(currentUser);
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "AdminManagerAccess")]
        [Route("profile/update")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUser = await _userService.GetCurrentUser();
            currentUser.FullName = model.FullName;
            await _userService.UpdateUser(currentUser);
            return RedirectToAction(nameof(AdminController.Profile), "Admin");
        }

        [HttpGet]
        [Authorize(Policy = "AdminManagerAccess")]
        [Route("profile/change-password")]
        public IActionResult ChangePassword()
        {
            var model = new ChangePasswordViewModel();
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "AdminManagerAccess")]
        [Route("profile/change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.NewPassword == model.CurrentPassword)
            {
                ModelState.AddModelError("NewPassword", ValidationMessages.NewPasswordError);
                return View(model);
            }

            var currentUser = await _userService.GetCurrentUser();
            var changedPassword = await _userService.ChangePassword(currentUser.UserName, model.CurrentPassword, model.NewPassword);
            if (!changedPassword)
            {
                ModelState.AddModelError("CurrentPassword", ValidationMessages.CurrentPasswordError);
                return View(model);
            }

            return RedirectToAction(nameof(AdminController.Profile), "Admin");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login(string returnUrl = "/admin")
        {
            if (_userService.IsSignedIn(User) && !_userService.IsInRole(_userService.GetCurrentUser().Result, RoleName.Customer).Result)
            {
                return RedirectToAction(nameof(AdminController.Task), "Admin");
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;

            if (User.Identity.IsAuthenticated)
            {
                return Redirect(returnUrl);
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Route("login")]
        public async Task<IActionResult> Login(AdminLoginViewModel model, string returnUrl = "/admin")
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userService.GetUserByName(model.UserName);
            if (user != null && !_userService.IsInRole(user, RoleName.Customer).Result)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _userService.Login(model.UserName, model.Password, model.RememberMe, true);
                if (result == LoginStatus.Succeeded)
                {
                    return Redirect(returnUrl);
                }
                else if (result == LoginStatus.IsLockedOut)
                {
                    var timeLockedOut = user.LockoutEnd == null ? 60 : (user.LockoutEnd.Value.LocalDateTime - DateTime.Now).Seconds;
                    ViewData["Message"] = string.Format(ValidationMessages.AccountLockedOut, timeLockedOut < 0 ? 60 : timeLockedOut);
                    return View(model);
                }

            }

            ViewData["Message"] = ValidationMessages.LoginFailed;
            return View(model);
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutUser();
            return RedirectToAction(nameof(AdminController.Login), "Admin");
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByName(model.Email);
                if (user == null || !user.EmailConfirmed || _userService.IsInRole(user, RoleName.Customer).Result)
                {
                    ViewData["Message"] = ValidationMessages.AccountNotRegister;
                    return View(model);
                }

                var sendMail = await SendEmailResetPassword(user);
                if (sendMail)
                {
                    ViewData["Message"] = ValidationMessages.ForgotPasswordConfirm;
                }
                else
                {
                    ViewData["Message"] = ValidationMessages.SendMailError;
                }

                //return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("forgot-password-confirm")]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("reset-password")]
        public IActionResult ResetPassword(string userName = null, string code = null)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(code))
            {
                return View("Error");
            }
            var model = new ResetPasswordViewModel { Code = code, Email = userName };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _userService.ResetPassword(model.Email, model.Code, model.Password);
            if (result)
            {
                return RedirectToAction(nameof(AdminController.Task), "Admin");
            }

            ViewData["Message"] = ValidationMessages.ResetPasswordError;
            return View();
        }

        private MailConfig SetMailConfig(string to, string subject, string body)
        {
            return new MailConfig()
            {
                EmailFrom = _configuration.GetValue<string>("EmailConfig:EmailFrom"),
                EnableSsl = _configuration.GetValue<bool>("EmailConfig:EnableSsl"),
                Port = _configuration.GetValue<int>("EmailConfig:Port"),
                Host = _configuration.GetValue<string>("EmailConfig:Host"),
                Username = _configuration.GetValue<string>("EmailConfig:Username"),
                Password = _configuration.GetValue<string>("EmailConfig:Password"),
                EmailSenderName = _configuration.GetValue<string>("EmailConfig:EmailSenderName"),
                EmailSubject = subject,
                MailTo = to,
                EmailBody = body
            };
        }
        private async Task<bool> SendEmailResetPassword(UserModel userModel)
        {
            var code = await _userService.GeneratePasswordResetToken(userModel);
            var userName = userModel.UserName;
            var callbackUrl = Url.Action(
                                    action: nameof(AdminController.ResetPassword),
                                    controller: "Admin",
                                    values: new { userName, code },
                                    protocol: Request.Scheme);

            var body = _configuration.GetValue<string>("EmailBody:ResetPassword").Replace("[FullName]", userModel.FullName).Replace("[CallBackUrl]", callbackUrl);
            var mailConfig = SetMailConfig(userModel.UserName, _configuration.GetValue<string>("EmailSubject:ResetPassword"), body);

            return _emailSender.SendEmail(mailConfig);
        }
    }
}
