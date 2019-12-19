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
    public class SublocalitiesController : ControllerBase
    {
        private readonly ISublocalityService _sublocalityService;
        public SublocalitiesController(ISublocalityService sublocalityService)
        {
            _sublocalityService = sublocalityService;
        }

        /// <summary>
        /// Get sublocality only by list localityid
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetSublocalityByListLocalityId(SublocalitySearchModel model)
        {
            var result = await _sublocalityService.GetSublocalityByListLocalityId(new SublocalitySearch{LocalityIds = model.LocalityIds});

            return Ok(result);
        }
    }
}