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
    [Route("page")]
    public class PageController : Controller
    {
        private readonly IConfiguration _configuration;

        public PageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Route("charge-list")]
        [HttpGet]
        public IActionResult ChargeList()
        {
            return View();
        }

        [Route("fee-table")]
        [HttpGet]
        public IActionResult FeeTable()
        {
            return View();
        }
    }
}
