using Microsoft.AspNetCore.Mvc;

namespace smartFunds.Presentation.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
