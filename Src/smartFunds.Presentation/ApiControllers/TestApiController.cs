using Microsoft.AspNetCore.Mvc;
using smartFunds.Model.Common;
using smartFunds.Service.Services;
using System.Threading.Tasks;

namespace smartFunds.Presentation.ApiControllers
{
    /// <summary>
    /// This is a test api controller that demo the flow of application.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TestApiController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestApiController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTest(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string test = await _testService.GetTest(id);
            return Ok(test);
        }

        [HttpPost]
        [Route("savetest")]
        public async Task<IActionResult> SaveTest(TestModel testModel)
        {
            var savedTest = await _testService.SaveTest(testModel);
            return Ok(savedTest);
        }

    }
}
