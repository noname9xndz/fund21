using smartFunds.Service.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace smartFunds.Service.Services
{
    public interface IUserService
    {
        User GetCurrentUser();
    }
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public User GetCurrentUser()
        {
            if (_httpContextAccessor?.HttpContext?.User == null)
                return null;

            var userContext = _httpContextAccessor.HttpContext.User;

            return new User()
            {
                UserName = userContext.Identity?.Name,
                MemberId = int.Parse(userContext.FindFirst(x => x.Type == "MemberId")?.Value),
                Roles = userContext.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList()
            };

        }
    }
}