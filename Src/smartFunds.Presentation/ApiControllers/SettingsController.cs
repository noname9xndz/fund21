using smartFunds.Presentation.Models;
using smartFunds.Caching;
using smartFunds.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingService _settingService;
        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;;
        }

        // GET: api/settings
        [HttpGet]
        public async Task<ActionResult<GetSettingViewModel>> GetSetting()
        {
            return new GetSettingViewModel
            {
                Setting = await _settingService.GetAll()
            };
        }
    }
}