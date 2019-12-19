using smartFunds.Service.Models;
using smartFunds.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InterchangesController : ControllerBase
    {
        private readonly IInterchangeService _interchangeService;
        private readonly IUserService _userService;
        public InterchangesController(IInterchangeService interchangeService, IUserService userService)
        {
            _interchangeService = interchangeService;
            _userService = userService;
        }

        /// <summary>
        /// GET: api/Interchange/5
        /// Get interchange information by localityId
        /// </summary>
        /// <param name="localityId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Get(int localityId)
        {
            var result = await _interchangeService.GetInterchangeByLocality(localityId);

            return Ok(result);
        }

        // POST: api/Interchange
        [HttpPost]
        public async Task Post([FromBody] InterchangeInputModel interchange)
        {
            await _interchangeService.AddOrUpdateInterchangeAsync(new Interchange
            {
                MainLocalityId = interchange.MainLocalityId,
                CountryCode = interchange.CountryCode,
                Localities = interchange.LocalityIds.Select(x => new Locality
                {
                    Id = x
                }).ToList(),
                EmailAddress = interchange.EmailAddress ?? string.Empty
            });
        }

        /// <summary>
        /// Get init data for member
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("initData")]
        public async Task<IActionResult> InitMemberData()
        {
            var result = await _interchangeService.GetMemberInterchangeData(_userService.GetCurrentUser());

            return Ok(result);
        }
    }
}
