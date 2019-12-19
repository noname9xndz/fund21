using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        // GET: api/authentication/testToken
        [Authorize]
        [Route("testToken")]
        [HttpGet]
        public async Task<ActionResult<bool>> TestToken()
        {
            return await Task.FromResult(true);
        }

        // GET: api/authentication/testAdminToken
        [Authorize(Roles = "ADMIN")]
        [Route("testAdminToken")]
        [HttpGet]
        public async Task<ActionResult<bool>> TestAdminToken()
        {
            return await Task.FromResult(true);
        }
    }
}