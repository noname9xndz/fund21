using Microsoft.AspNetCore.Mvc;
using smartFunds.Service.Services;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            if (_userService.IsSignedIn(User) && !_userService.IsInRole(_userService.GetCurrentUser().Result, RoleName.Customer).Result)
            {
                return RedirectToAction(nameof(AdminController.Task), "Admin");
            }

            return View();
        }

        
    }
}
