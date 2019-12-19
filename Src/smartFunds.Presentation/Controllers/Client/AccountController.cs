using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using smartFunds.Common;
using smartFunds.Common.Helpers;
using smartFunds.Infrastructure.Models;
using smartFunds.Infrastructure.Services;
using smartFunds.Model.Common;
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;
using System.Threading.Tasks;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;
        private readonly ISMSGateway _smsGateway;
        private readonly IConfiguration _configuration;

        public AccountController(IUserService userService, IEmailSender emailSender, IConfiguration configuration, ISMSGateway smsGateway)
        {
            _userService = userService;
            _emailSender = emailSender;
            _configuration = configuration;
            _smsGateway = smsGateway;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid && model != null && (model.EmailOrPhone.IsEmail() || model.EmailOrPhone.IsPhoneNumber()))
            {
                var userModel = new UserModel { UserName = model.EmailOrPhone, FullName = model.FullName };
                if (model.EmailOrPhone.IsEmail())
                {
                    userModel.Email = model.EmailOrPhone;
                }
                else
                {
                    userModel.PhoneNumber = model.EmailOrPhone;
                }

                var result = await _userService.RegisterUser(userModel, model.Password, RoleName.Customer);
                if (result == RegisterStatus.ExistUser)
                {
                    ViewData["Message"] = "exist user";
                    return RedirectToLocal("/account/login");
                }
                if (result == RegisterStatus.Succeeded)
                {
                    if (userModel.UserName.IsEmail())
                    {
                        var sendEmail = await SendEmailConfirmLink(userModel);
                        if (sendEmail)
                        {
                            var userName = userModel.UserName;
                            return RedirectToAction(nameof(AccountController.Active), "Account", new { userName });
                        }
                        else
                        {

                        }
                    }
                    else if (userModel.UserName.IsPhoneNumber())
                    {
                        var sendSMS = await SendSMSVerifyCode(userModel.UserName, model.EmailOrPhone);
                        var userName = userModel.UserName;
                        return RedirectToAction(nameof(AccountController.Active), "Account", new { userName });
                    }
                }
                else if (result == RegisterStatus.NotVerify)
                {
                    var userName = userModel.UserName;
                    return RedirectToAction(nameof(AccountController.ResendCode), "Account", new { userName });
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var result = await _userService.ConfirmEmail(userId, code);
            if (result)
            {
                //return View("ConfirmEmail");
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                var user = await _userService.GetUserById(userId);
                if(user != null)
                {
                    var userName = user.UserName;
                    return RedirectToAction(nameof(AccountController.ResendCode), "Account", new { userName });
                }
            }
            return View("Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResendCode()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResendCode(string userName)
        {
            if (userName == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            if(userName.IsEmail())
            {
                var userModel = await _userService.GetUserByName(userName);

                if (userModel == null)
                {
                    return View("Error");
                }
                var sendEmail = await SendEmailConfirmLink(userModel);
                if (sendEmail)
                {
                    return RedirectToAction(nameof(AccountController.Active), "Account", new { userName });
                }
                else
                {

                }
            }
            else if(userName.IsPhoneNumber())
            {
                var sendSMS = await SendSMSVerifyCode(userName, userName);
                if (sendSMS)
                {
                    return RedirectToAction(nameof(AccountController.Active), "Account", new { userName });
                }
                else
                {

                }
            }
            
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
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
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(model.UserName) && !string.IsNullOrWhiteSpace(model.Password))
            {
                var user = await _userService.GetUserByName(model.UserName);
                if(user != null && await _userService.IsInRole(user, RoleName.Customer))
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _userService.Login(model.UserName, model.Password, model.RememberMe, false);
                    if (result == LoginStatus.Succeeded)
                    {
                        return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                    else if (result == LoginStatus.NotVerify)
                    {
                        var userName = model.UserName;
                        return RedirectToAction(nameof(AccountController.ResendCode), "Account", new { userName });
                    }
                    else
                    {
                        
                    }
                }
                else
                {

                }
                
            }

            ModelState.AddModelError(string.Empty, "Sai tai khoan hoac mat khau");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutUser();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Active(string userName)
        {
            if(string.IsNullOrWhiteSpace(userName) || (!userName.IsEmail() && !userName.IsPhoneNumber()))
            {
                return View("Error");
            }
            var model = new ActiveViewModel();
            model.UserName = userName;
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Active(ActiveViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.UserName) || (!model.UserName.IsEmail() && !model.UserName.IsPhoneNumber()))
            {
                return View("Error");
            }

            var result = await _userService.ConfirmPhone(model.UserName, model.VerifyCode, model.UserName);
            if (result)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Sai ma kich hoat hoac ma kich hoat da het han");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.GetUserByName(model.EmailOrPhone);
                if (user == null || (!user.EmailConfirmed && !user.PhoneNumberConfirmed))
                {
                    ModelState.AddModelError(string.Empty, "Tai khoan chua dang ky, hoac chua duoc kich hoat");
                    return View(model);
                }

                if(model.EmailOrPhone.IsEmail())
                {
                    var sendMail = await SendEmailResetPassword(user);
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }
                else
                {
                    var sendMail = await SendSMSVerifyCode(model.EmailOrPhone, model.EmailOrPhone);
                    var userName = model.EmailOrPhone;
                    return RedirectToAction(nameof(ResetPasswordByVerifyCode), new { userName });
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string userName = null, string code = null)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(code))
            {
                return View("Error");
            }
            var model = new ResetPasswordViewModel { Code = code, EmailOrPhone = userName };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.EmailOrPhone))
            {
                ModelState.AddModelError(string.Empty, "Sai tai khoan");
                return View(model);
            }

            var result = await _userService.ResetPassword(model.EmailOrPhone, model.Code, model.Password);
            if (result)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            ModelState.AddModelError(string.Empty, "Loi trong qua trinh tao lai mat khau");
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordByVerifyCode(string userName = null)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return View("Error");
            }
            var model = new ResetPasswordViewModel { EmailOrPhone = userName };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPasswordByVerifyCode(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.EmailOrPhone))
            {
                ModelState.AddModelError(string.Empty, "Sai tai khoan");
                return View(model);
            }
            //sms reset passs
            var result = await _userService.ResetPasswordByVerifyCode(model.EmailOrPhone, model.Code, model.EmailOrPhone, model.Password);
            if (result)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            ModelState.AddModelError(string.Empty, "Loi trong qua trinh tao lai mat khau");
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
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
        private async Task<bool> SendEmailConfirmLink(UserModel userModel)
        {
            var code = await _userService.GenerateEmailConfirmationToken(userModel);
            var userId = userModel.Id;
            var callbackUrl = Url.Action(
                                    action: nameof(AccountController.ConfirmEmail),
                                    controller: "Account",
                                    values: new { userId, code },
                                    protocol: Request.Scheme);

            var mailConfig = SetMailConfig(userModel.UserName, _configuration.GetValue<string>("EmailSubject:ConfirmAccount"), _configuration.GetValue<string>("EmailBody:ConfirmAccount").Replace("[CallBackUrl]", callbackUrl));
            
            return _emailSender.SendEmail(mailConfig);
        }

        private async Task<bool> SendEmailResetPassword(UserModel userModel)
        {
            var code = await _userService.GeneratePasswordResetToken(userModel);
            var userName = userModel.UserName;
            var callbackUrl = Url.Action(
                                    action: nameof(AccountController.ResetPassword),
                                    controller: "Account",
                                    values: new { userName, code },
                                    protocol: Request.Scheme);

            var mailConfig = SetMailConfig(userModel.UserName, _configuration.GetValue<string>("EmailSubject:ResetPassword"), _configuration.GetValue<string>("EmailBody:ResetPassword").Replace("[CallBackUrl]", callbackUrl));

            return _emailSender.SendEmail(mailConfig);
        }

        private SMSConfig SetSMSConfig(string phone, string message)
        {
            return new SMSConfig()
            {
                APIKey = _configuration.GetValue<string>("SMSConfig:APIKey"),
                SecretKey = _configuration.GetValue<string>("SMSConfig:SecretKey"),
                IsFlash = _configuration.GetValue<bool>("SMSConfig:IsFlash"),
                BrandName = _configuration.GetValue<string>("SMSConfig:BrandName"),
                SMSType = _configuration.GetValue<int>("SMSConfig:SMSType"),
                Phone = phone,
                Message = message
            };
        }
        private async Task<bool> SendSMSVerifyCode(string userName, string phoneNumber)
        {
            var code = await _userService.GeneratePhoneConfirmationToken(userName, phoneNumber);

            var smsConfig = SetSMSConfig(phoneNumber, _configuration.GetValue<string>("SMSConfig:Message").Replace("[VerifyCode]", code));

            return _smsGateway.Send(smsConfig);
        }

    }
}
