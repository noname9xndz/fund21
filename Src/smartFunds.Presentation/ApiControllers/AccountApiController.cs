using Microsoft.AspNetCore.Mvc;
using smartFunds.Model.Common;
using smartFunds.Presentation.Models;
using smartFunds.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Presentation.ApiControllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountApiController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            //var userModel = new UserModel() { UserName = model.EmailOrPhone, Email = model.EmailOrPhone, FullName = model.FullName };
            //var result = await _userService.RegisterUser(userModel, model.Password);
            //if(result)
            //{
            //    return Ok(userModel);
            //}

            return NoContent();
        }

        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _userService.GetUserById(id);
            return Ok(user);
        }
    }
}
