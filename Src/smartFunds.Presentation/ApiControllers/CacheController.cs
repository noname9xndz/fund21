using smartFunds.Common.Options;
using smartFunds.Service.Models;
using smartFunds.Service.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly ICacheService _cacheService;
        private readonly IOptions<AppSettingsOptions> _appSettingsOptions;
        
        public CacheController(ICacheService cacheService, IOptions<AppSettingsOptions> appSettingsOptions)
        {
            _cacheService = cacheService;
            _appSettingsOptions = appSettingsOptions;            
        }

        // GET: api/cache/build
        [HttpGet("build")]
        public async Task<ActionResult<UserQueuedJob>> Build(string key)
        {
            if (key != _appSettingsOptions.Value.SecureAccessKey)
            {
                return Unauthorized();
            }

            var canQueue = await _cacheService.SetBuildCacheStatusToQueue();
            if (canQueue)
            {
                //await _userQueuedJobService.EnqueueAsync(_appSettingsOptions.Value.SecureAccessKey, () => _cacheService.BuildCache());
                return Ok();
            }

            return Conflict();
        }
    }
}