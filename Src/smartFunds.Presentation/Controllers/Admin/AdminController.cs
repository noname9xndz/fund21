using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using smartFunds.Common;
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;
using System.Threading.Tasks;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = "/")
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;
            
            if(User.Identity.IsAuthenticated)
            {
                return Redirect(returnUrl);
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid && !string.IsNullOrWhiteSpace(model.UserName) && !string.IsNullOrWhiteSpace(model.Password))
            {
                var user = await _userService.GetUserByName(model.UserName);
                if (user != null && (_userService.IsInRole(user, RoleName.Admin).Result || _userService.IsInRole(user, RoleName.Manager).Result))
                {
                    // This doesn't count login failures towards account lockout
                    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                    var result = await _userService.Login(model.UserName, model.Password, model.RememberMe, false);
                    if (result == LoginStatus.Succeeded)
                    {
                        return Redirect(returnUrl);
                    }
                    
                }
            }

            ModelState.AddModelError(string.Empty, "Sai tai khoan hoac mat khau");
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _userService.LogoutUser();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
