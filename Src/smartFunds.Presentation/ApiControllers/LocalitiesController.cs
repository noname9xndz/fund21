using System.Threading.Tasks;
using smartFunds.Presentation.Models;
using smartFunds.Service.Models;
using smartFunds.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace smartFunds.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocalitiesController : ControllerBase
    {
        private readonly ILocalityService _localityService;
        public LocalitiesController(ILocalityService localityService)
        {
            _localityService = localityService;
        }

        /// <summary>
        /// Get all localities under current country
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetLocalityByCountryCode(string countryCode)
        {
            var result = await _localityService.GetLocalityByCountryCode(countryCode);

            return Ok(result);
        }

        /// <summary>
        /// Get locality only by list country code
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetLocalityByCountryCodes(LocalitySearchModel model)
        {
            var result = await _localityService.GetLocalityByListCountryCode(new LocalitySearch{CountryCodes = model.CountryCodes});

            return Ok(result);
        }
    }
}