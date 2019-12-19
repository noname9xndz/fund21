using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models;
using smartFunds.Data.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User InitBasicInfo(User user);
    }

    public class UserRepository : GenericPersistentTrackedRepository<User>, IUserRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public UserRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider, IHttpContextAccessor httpContextAccessor) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider, httpContextAccessor)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        public User InitBasicInfo(User user)
        {
            var newUser = user;
            newUser.InitialInvestmentAmount = 0;
            newUser.CurrentAccountAmount = 0;
            newUser.Created = DateTime.Now;
            newUser.IsActive = true;
            newUser.DateLastUpdated = DateTime.Now;
            newUser.LastUpdatedBy = CurrentUser;
            return newUser;
        }
    }
}
