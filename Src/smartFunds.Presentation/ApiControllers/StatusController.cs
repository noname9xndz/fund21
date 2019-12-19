using smartFunds.Presentation.Models;
using smartFunds.Common.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Reflection;

namespace smartFunds.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly IOptions<AppSettingsOptions> _appSettingsOptions;
        private readonly IOptions<ConnectionStringsOptions> _connectionStringsOptions;
        public StatusController(IOptions<AppSettingsOptions> appSettingsOptions, IOptions<ConnectionStringsOptions> connectionStringsOptions)
        {
            _appSettingsOptions = appSettingsOptions;
            _connectionStringsOptions = connectionStringsOptions;
        }

        [HttpGet]
        [HttpPost]
        [HttpOptions]
        [Route("")]
        public ActionResult Ping()
        {
            return Ok("Ok");
        }

        [HttpGet]
        [HttpOptions]
        [Route("ping")]
        public ActionResult Ping(string key)
        {         
            if (key != _appSettingsOptions.Value.SecureAccessKey)
            {
                return Unauthorized();
            }

            var smartFundsConnectionString = new SqlConnectionStringBuilder(_connectionStringsOptions.Value.smartFundsDatabase);
            var contactBaseConnectionString = new SqlConnectionStringBuilder(_connectionStringsOptions.Value.ContactBaseDatabase);

            var result = new StatusModel
            {
                Status = "OK",
                Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                smartFundsDatabase = $"DataSource:{smartFundsConnectionString.DataSource}; InitialCatalog:{smartFundsConnectionString.InitialCatalog}; UserId: {smartFundsConnectionString.UserID}",            
                smartFundsContactBaseDatabase = $"DataSource:{contactBaseConnectionString.DataSource}; InitialCatalog:{contactBaseConnectionString.InitialCatalog}; UserId: {contactBaseConnectionString.UserID}"
            };

            return Ok(result);
        }

        [HttpGet]
        [Route("version")]
        public ActionResult GetVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var versionArray = assembly.GetName().Version.ToString().Split('.');
            return Ok($"{versionArray[0]}.{versionArray[1]}.{versionArray[2]}");
        }
    }
}