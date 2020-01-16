using System.Text;
using Microsoft.AspNetCore.Mvc;
using smartFunds.Model.Common;
using smartFunds.Service.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using smartFunds.Common;
using smartFunds.Common.Helpers;
using smartFunds.Infrastructure.Models;
using smartFunds.Infrastructure.Services;
using smartFunds.Presentation.Job;

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
        private readonly IConfiguration _configuration;
        private readonly IViettelPay _viettelPay;
        private readonly IOrderRequestService _orderRequestService;

        public TestApiController(ITestService testService, IConfiguration configuration, IViettelPay viettelPay, IOrderRequestService orderRequestService)
        {
            _testService = testService;
            _configuration = configuration;
            _viettelPay = viettelPay;
            _orderRequestService = orderRequestService;
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

        [HttpGet]
        [Route("testcheck")]
        public async Task<IActionResult> SaveTest(string orderRequest)
        {

             var viettelPayApi = _configuration.GetValue<bool>("RequestPaymentLink:IsLive") ? _configuration.GetValue<string>("RequestPaymentLink:Live") : _configuration.GetValue<string>("RequestPaymentLink:Test");
                var cmd = _configuration.GetValue<string>("RequestPaymentParam:cmdCheckOrderRequest");
                var rsaPublicKey = _configuration.GetValue<string>("RSAKey:public");
                var rsaPrivateKey = _configuration.GetValue<string>("RSAKey:private");
                var rsaPublicKeyVTP = _configuration.GetValue<string>("RSAKey:VTPpublic");

                var rsa = new RSAHelper(RSAType.RSA, Encoding.UTF8, "", rsaPublicKeyVTP);
                var passwordEncrypt = rsa.Encrypt(_configuration.GetValue<string>("RequestPaymentParam:password"));

                SoapDataCheckOrderRequest request = new SoapDataCheckOrderRequest()
                {
                    username = _configuration.GetValue<string>("RequestPaymentParam:username"),
                    password = passwordEncrypt,
                    serviceCode = _configuration.GetValue<string>("RequestPaymentParam:serviceCode"),
                    orderId = orderRequest
                };
                var response = await _viettelPay.CheckOrderRequest(viettelPayApi, cmd, rsaPublicKey, rsaPrivateKey, rsaPublicKeyVTP, request);

                return new OkObjectResult(response);

        }

    }
}
