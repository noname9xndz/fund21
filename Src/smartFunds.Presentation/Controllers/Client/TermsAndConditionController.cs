using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using smartFunds.Model.Common;
using smartFunds.Model.Resources;
using smartFunds.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Controllers.Client
{
    [Route("terms-and-condition")]
    public class TermsAndConditionController : Controller
    {
        private readonly IConfiguration _configuration;

        public TermsAndConditionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
