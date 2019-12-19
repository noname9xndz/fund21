using System.Threading.Tasks;
using smartFunds.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace smartFunds.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionService _regionService;
        public RegionsController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        /// <summary>
        /// Get all localities under current country
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetRegions()
        {
            var result = await _regionService.GetAllRegion();

            return Ok(result);
        }
    }
}