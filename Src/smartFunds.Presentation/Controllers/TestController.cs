using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using smartFunds.Model.Common;
using smartFunds.Service.Services;

namespace smartFunds.Presentation.Controllers
{
    /// <summary>
    /// This is a test controller that demo the flow of application.
    /// </summary>
    public class TestController : Controller
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }
        
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string test = await _testService.GetTest(id);

            TestModel model = new TestModel
            {
                Name = test
            };

            return View(model);
        }

        // GET: /<controller>/
        public IActionResult Index()
        {           
            return View();
        }
    }
}
