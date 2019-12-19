using System.Threading.Tasks;
using smartFunds.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace smartFunds.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;
        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        /// <summary>
        /// Get all additional country in the same region with current country
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAdditionalCountry(string countryCode)
        {
            var result = await _countryService.GetAllAdditionalCountry(countryCode);

            return Ok(result);
        }
    }
}