using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models;
using smartFunds.Data.Repositories.Generic;
using System;
using System.Linq;
using smartFunds.Model.Client;
using System.Collections.Generic;
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
            newUser.CurrentInvestmentAmount = -0.00001m;
            newUser.AdjustmentFactor = 1;
            newUser.CurrentAccountAmount = 0;
            newUser.Created = DateTime.Now;
            newUser.DateLastUpdated = DateTime.Now;
            newUser.LastUpdatedBy = CurrentUser;
            return newUser;
        }
    }
}
