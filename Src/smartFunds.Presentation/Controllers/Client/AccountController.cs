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
using Hangfire;
using smartFunds.Presentation.Controllers.Admin;

namespace smartFunds.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailSender _emailSender;
        private readonly ISMSGateway _smsGateway;
        private readonly IConfiguration _configuration;
        private readonly IKVRRService _kvrrService;
        private readonly IKVRRQuestionAnswerService _kVRRQuestionAnswerService;
        private readonly IFundTransactionHistoryService _fundTransactionHistoryService;

        public AccountController(IUserService userService, IEmailSender emailSender, IConfiguration configuration, ISMSGateway smsGateway, IKVRRService kvrrService, IKVRRQuestionAnswerService kVRRQuestionAnswerService, IFundTransactionHistoryService fundTransactionHistoryService)
        {
            _userService = userService;
            _emailSender = emailSender;
            _configuration = configuration;
            _smsGateway = smsGateway;
            _kvrrService = kvrrService;
            _kVRRQuestionAnswerService = kVRRQuestionAnswerService;
            _fundTransactionHistoryService = fundTransactionHistoryService;
        }

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult Register(string userName = null)
        //{
        //    if (_userService.IsSignedIn(User))
        //    {
        //        return RedirectToAction(nameof(HomeController.Index), "Home");
        //    }

        //    var model = new RegisterViewModel() { Phone = userName };
        //    return View(model);
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        //{
        //    ViewData["ReturnUrl"] = returnUrl;
        //    if (ModelState.IsValid && model != null)
        //    {
        //        var userModel = new UserModel { UserName = model.Phone, PhoneNumber = model.Phone, FullName = model.FullName };

        //        var result = await _userService.RegisterUser(userModel, StringHelper.GeneratePassword(true, false, true, true, false, 10), RoleName.Customer);
        //        if (result == RegisterStatus.ExistUser)
        //        {
        //            ViewData["Message"] = ValidationMessages.ExistedAccount;
        //            return View(model);
        //        }
        //        if (result == RegisterStatus.Succeeded)
        //        {
        //            var sendSMS = await SendSMSVerifyCode(userModel.UserName, model.Phone);
        //            if (sendSMS)
        //            {
        //                var userName = userModel.UserName;
        //                return RedirectToAction(nameof(AccountController.Active), "Account", new { userName });
        //            }
        //            else
        //            {
        //                ViewData["Message"] = ValidationMessages.SendSMSError;
        //                return View(model);
        //            }
        //        }
        //        else if (result == RegisterStatus.NotVerify)
        //        {
        //            var sendSMS = await SendSMSVerifyCode(userModel.UserName, model.Phone);
        //            if (sendSMS)
        //            {
        //                var userName = userModel.UserName;
        //                return RedirectToAction(nameof(AccountController.Active), "Account", new { userName });
        //            }
        //            else
        //            {
        //                ViewData["Message"] = ValidationMessages.SendSMSError;
        //                return View(model);
        //            }
        //        }

        //    }

        //    ViewData["Message"] = ValidationMessages.RegisterError;
        //    return View(model);
        //}


        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> ConfirmEmail(string userId, string code)
        //{
        //    if (userId == null || code == null)
        //    {
        //        return RedirectToAction(nameof(HomeController.Index), "Home");
        //    }

        //    var result = await _userService.ConfirmEmail(userId, code);
        //    if (result)
        //    {
        //        //return View("ConfirmEmail");
        //        return RedirectToAction(nameof(AccountController.DefindKVRR), "Account");
        //    }
        //    else
        //    {
        //        var user = await _userService.GetUserById(userId);
        //        if (user != null)
        //        {
        //            var userName = user.UserName;
        //            TempData["ResendCodeType"] = ResendCodeType.Expired;
        //            return RedirectToAction(nameof(AccountController.ResendCode), "Account", new { userName });
        //        }
        //    }
        //    return RedirectToAction(nameof(AccountController.Register), "Account");
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult ResendCode()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> ResendCode(string userName)
        //{
        //    if (userName == null)
        //    {
        //        return RedirectToAction(nameof(HomeController.Index), "Home");
        //    }

        //    if (userName.IsPhoneNumber())
        //    {
        //        var sendSMS = await SendSMSVerifyCode(userName, userName);
        //        if (sendSMS)
        //        {
        //            return RedirectToAction(nameof(AccountController.Active), "Account", new { userName });
        //        }
        //        else
        //        {
        //            ViewData["Message"] = ValidationMessages.SendSMSError;
        //        }
        //    }

        //    return View();
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> Login(string returnUrl = null)
        //{
        //    if (_userService.IsSignedIn(User))
        //    {
        //        return RedirectToAction(nameof(HomeController.Index), "Home");
        //    }

        //    // Clear the existing external cookie to ensure a clean login process
        //    await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        //    ViewData["ReturnUrl"] = returnUrl;

        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return Redirect(returnUrl);
        //    }

        //    return View();
        //}

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoginByViettelPay(string merchant_code, string msisdn, string time, string check_sum)
        {

            if (string.IsNullOrWhiteSpace(msisdn) || string.IsNullOrWhiteSpace(check_sum) || !msisdn.IsPhoneNumber())
            {
                var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
                logger.Error("Login By ViettelPay: merchant_code: " + merchant_code + ", msisdn: " + msisdn + ", time: " + time + ", check_sum: " + check_sum + ", error_msg: Parametter Invalid");
                return RedirectToAction(nameof(ErrorController.Error404), "Error");
            }

            var checkSum = Helpers.CreateCheckSumSHA256(_configuration.GetValue<string>("PaymentSecurity:AppKey"),
                   _configuration.GetValue<string>("PaymentParam:vt_merchant_code"), msisdn, time);
            var currentCheckSum = check_sum.Replace("-", "").ToLower();
            if (checkSum != currentCheckSum)
            {
                var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
                logger.Error("Login By ViettelPay: merchant_code: " + merchant_code + ", msisdn: " + msisdn + ", time: " + time + ", check_sum: " + check_sum + ", error_msg: check_sum Invalid");

                return RedirectToAction(nameof(ErrorController.Error404), "Error");
            }

            var user = await _userService.GetUserByEmailOrPhone(msisdn);

            if (user != null && await _userService.IsInRole(user, RoleName.Customer))
            {
                if (_userService.IsSignedIn(User))
                {
                    if (User.Identity.Name == user.UserName)
                    {
                        return RedirectToAction(nameof(AccountController.MyPortfolio), "Account");
                    }
                    else
                    {
                        await _userService.LogoutUser();
                    }
                }

                await _userService.Login(user.UserName);
                return RedirectToAction(nameof(AccountController.MyPortfolio), "Account");
            }
            else
            {
                if (_userService.IsSignedIn(User))
                {
                    await _userService.LogoutUser();
                }

                var userModel = new UserModel { UserName = msisdn, PhoneNumber = msisdn, FullName = msisdn };
                var result = await _userService.RegisterUser(userModel, StringHelper.GeneratePassword(true, false, true, true, false, 10), RoleName.Customer);

                await _userService.Login(userModel.UserName);
                return RedirectToAction(nameof(AccountController.EditName), "Account");
            }
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        //{
        //    ViewData["ReturnUrl"] = returnUrl;
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userService.GetUserByEmailOrPhone(model.UserName);
        //        if (user != null && await _userService.IsInRole(user, RoleName.Customer))
        //        {
        //            var sendSMS = await SendSMSVerifyCode(model.UserName, model.UserName);
        //            if (sendSMS)
        //            {
        //                var userName = model.UserName;
        //                return RedirectToAction(nameof(AccountController.Active), "Account", new { userName });
        //            }
        //            else
        //            {
        //                ViewData["Message"] = ValidationMessages.SendSMSError;
        //                return View(model);
        //            }
        //        }
        //        else
        //        {
        //            TempData["Message"] = ValidationMessages.AccountNotRegister;
        //            var userName = model.UserName;
        //            return RedirectToAction(nameof(AccountController.Register), "Account", new { userName });
        //        }
        //    }

        //    return View(model);
        //}

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutUser();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult Active(string userName)
        //{
        //    if (string.IsNullOrWhiteSpace(userName) || (!userName.IsEmail() && !userName.IsPhoneNumber()))
        //    {
        //        return View("Error");
        //    }
        //    var model = new ActiveViewModel();
        //    model.UserName = userName;
        //    return View(model);
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<IActionResult> Active(ActiveViewModel model, string returnUrl = null)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    if (string.IsNullOrWhiteSpace(model.UserName) || (!model.UserName.IsEmail() && !model.UserName.IsPhoneNumber()))
        //    {
        //        return RedirectToAction(nameof(HomeController.Index), "Home");
        //    }
        //    var currentUser = await _userService.GetUserByName(model.UserName);
        //    var result = await _userService.ConfirmPhone(model.UserName, model.VerifyCode, model.UserName);
        //    if (result)
        //    {
        //        if (currentUser.LastLogin < currentUser.Created)
        //        {
        //            return RedirectToAction(nameof(AccountController.DefindKVRR), "Account");
        //        }

        //        return RedirectToAction(nameof(AccountController.MyWallet), "Account");
        //    }
        //    else
        //    {
        //        ViewData["Message"] = ValidationMessages.OTPFailed;
        //    }

        //    return View(model);
        //}

        [HttpGet]
        public IActionResult EditName()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditName(EditNameViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUser = await _userService.GetCurrentUser();

            currentUser.FullName = model.FullName;

            await _userService.UpdateUser(currentUser);

            return RedirectToAction(nameof(AccountController.DefindKVRR), "Account");
        }

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult ForgotPassword()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userService.GetUserByName(model.Email);
        //        if (user == null || (!user.EmailConfirmed && !user.PhoneNumberConfirmed))
        //        {
        //            ModelState.AddModelError(string.Empty, "Tai khoan chua dang ky, hoac chua duoc kich hoat");
        //            return View(model);
        //        }

        //        if (model.Email.IsEmail())
        //        {
        //            var sendMail = await SendEmailResetPassword(user);
        //            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        //        }
        //        else
        //        {
        //            var sendMail = await SendSMSVerifyCode(model.Email, model.Email);
        //            var userName = model.Email;
        //            return RedirectToAction(nameof(ResetPasswordByVerifyCode), new { userName });
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult ForgotPasswordConfirmation()
        //{
        //    return View();
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult ResetPassword(string userName = null, string code = null)
        //{
        //    if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(code))
        //    {
        //        return View("Error");
        //    }
        //    var model = new ResetPasswordViewModel { Code = code, Email = userName };
        //    return View(model);
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    if (string.IsNullOrWhiteSpace(model.Email))
        //    {
        //        ModelState.AddModelError(string.Empty, "Sai tai khoan");
        //        return View(model);
        //    }

        //    var result = await _userService.ResetPassword(model.Email, model.Code, model.Password);
        //    if (result)
        //    {
        //        return RedirectToAction(nameof(HomeController.Index), "Home");
        //    }
        //    ModelState.AddModelError(string.Empty, "Loi trong qua trinh tao lai mat khau");
        //    return View();
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult ResetPasswordConfirmation()
        //{
        //    return View();
        //}

        //[HttpGet]
        //[AllowAnonymous]
        //public IActionResult ResetPasswordByVerifyCode(string userName = null)
        //{
        //    if (string.IsNullOrWhiteSpace(userName))
        //    {
        //        return View("Error");
        //    }
        //    var model = new ResetPasswordViewModel { Email = userName };
        //    return View(model);
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> ResetPasswordByVerifyCode(ResetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    if (string.IsNullOrWhiteSpace(model.Email))
        //    {
        //        ModelState.AddModelError(string.Empty, "Sai tai khoan");
        //        return View(model);
        //    }
        //    //sms reset passs
        //    var result = await _userService.ResetPasswordByVerifyCode(model.Email, model.Code, model.Email, model.Password);
        //    if (result)
        //    {
        //        return RedirectToAction(nameof(HomeController.Index), "Home");
        //    }
        //    ModelState.AddModelError(string.Empty, "Loi trong qua trinh tao lai mat khau");
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> MyWallet()
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            UserModel model = new UserModel();
            model = await _userService.GetCurrentUser();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DefindKVRR()
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            try
            {
                var questions = await _kVRRQuestionAnswerService.GetKVRRQuestions() ?? null;
                var result = questions.OrderBy(x => x.No).ToList();
                return View(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDefindKVRR(List<KVRRQuestion> questions)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<int> ids = new List<int>();
                    if (questions == null || !questions.Any()) throw new InvalidParameterException();
                    foreach (var question in questions)
                    {
                        if (!string.IsNullOrEmpty(question.AnswerSelected))
                            ids.Add(int.Parse(question.AnswerSelected));
                    }

                    var kvrr = await _kvrrService.GetKVRRByMark(ids);
                    return RedirectToAction("KVRRRecommendation", kvrr);
                }
                else
                {
                    return RedirectToAction("DefindKVRR");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> KVRRRecommendation(KVRRModel kvrrModel)
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await _userService.GetCurrentUser();

            UserModel model = new UserModel();
            model = await _userService.GetUserRelateData();
            if (currentUser.KVRR != null && kvrrModel != null && currentUser.KVRR.Id == kvrrModel.Id)
            {
                model.KVRR = new KVRRModel()
                {
                    Id = 0
                };
            }
            else
            {
                model.KVRR = kvrrModel;
            }
            var kvrrs = await _kvrrService.GetAllKVRR();
            if (currentUser.KVRR != null)
            {
                model.KVRROthers = kvrrs?.Where(x => model.KVRR != null && x.Id != model.KVRR.Id && x.Id != currentUser.KVRR.Id)?.ToList() ?? throw new NotFoundException();
            }
            else
            {
                model.KVRROthers = kvrrs?.Where(x => model.KVRR != null && x.Id != model.KVRR.Id)?.ToList() ?? throw new NotFoundException();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmKVRR(List<string> SelectedKVRRId)
        {
            try
            {
                if (SelectedKVRRId == null || !SelectedKVRRId.Any()) throw new InvalidParameterException();

                var id = SelectedKVRRId.FirstOrDefault(x => !string.IsNullOrEmpty(x));

                await _fundTransactionHistoryService.ChangeKVRR(Int32.Parse(id));
                return RedirectToAction("MyPortfolio");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public async Task<IActionResult> KVRRSelection()
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var currentUser = await _userService.GetCurrentUser();

            UserModel model = new UserModel();
            model = await _userService.GetUserRelateData();
            var kvrrs = await _kvrrService.GetAllKVRR();
            if (currentUser.KVRR != null)
            {
                model.KVRROthers = kvrrs?.Where(x => x.Id != currentUser.KVRR.Id).ToList() ?? throw new NotFoundException();
            }
            else
            {
                model.KVRROthers = kvrrs?.ToList() ?? throw new NotFoundException();
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> MyPortfolio()
        {
            if (!_userService.IsSignedIn(User))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            UserPorfolioModel model = new UserPorfolioModel();
            List<UserPorfolio> userPorfolios = await _userService.GetUserPorfolio();

            model.UserPortfolios = userPorfolios;
            model.CurrentUser = await _userService.GetCurrentUser();
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PaymentResult()
        {
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
        //private async Task<bool> SendEmailConfirmLink(UserModel userModel)
        //{
        //    var user = await _userService.GetUserByName(userModel.UserName);
        //    var code = await _userService.GenerateEmailConfirmationToken(user);
        //    var userId = user.Id;
        //    var callbackUrl = Url.Action(
        //                            action: nameof(AccountController.ConfirmEmail),
        //                            controller: "Account",
        //                            values: new { userId, code },
        //                            protocol: Request.Scheme);

        //    var body = _configuration.GetValue<string>("EmailBody:ConfirmAccount").Replace("[FullName]", user.FullName).Replace("[CallBackUrl]", callbackUrl);
        //    var mailConfig = SetMailConfig(userModel.UserName, _configuration.GetValue<string>("EmailSubject:ConfirmAccount"), body);

        //    return _emailSender.SendEmail(mailConfig);
        //}

        //private async Task<bool> SendEmailResetPassword(UserModel userModel)
        //{
        //    var code = await _userService.GeneratePasswordResetToken(userModel);
        //    var userName = userModel.UserName;
        //    var callbackUrl = Url.Action(
        //                            action: nameof(AccountController.ResetPassword),
        //                            controller: "Account",
        //                            values: new { userName, code },
        //                            protocol: Request.Scheme);

        //    var mailConfig = SetMailConfig(userModel.UserName, _configuration.GetValue<string>("EmailSubject:ResetPassword"), _configuration.GetValue<string>("EmailBody:ResetPassword").Replace("[CallBackUrl]", callbackUrl));

        //    return _emailSender.SendEmail(mailConfig);
        //}

        //private SMSConfig SetSMSConfig(string phone, string message)
        //{
        //    return new SMSConfig()
        //    {
        //        APIKey = _configuration.GetValue<string>("SMSConfig:APIKey"),
        //        SecretKey = _configuration.GetValue<string>("SMSConfig:SecretKey"),
        //        IsFlash = _configuration.GetValue<bool>("SMSConfig:IsFlash"),
        //        BrandName = _configuration.GetValue<string>("SMSConfig:BrandName"),
        //        SMSType = _configuration.GetValue<int>("SMSConfig:SMSType"),
        //        Phone = phone,
        //        Message = message
        //    };
        //}
        //private async Task<bool> SendSMSVerifyCode(string userName, string phoneNumber)
        //{
        //    var code = await _userService.GeneratePhoneConfirmationToken(userName, phoneNumber);

        //    var smsConfig = SetSMSConfig(phoneNumber, _configuration.GetValue<string>("SMSConfig:Message").Replace("[VerifyCode]", code));

        //    //return _smsGateway.Send(smsConfig);
        //    return true;
        //}

    }
}
