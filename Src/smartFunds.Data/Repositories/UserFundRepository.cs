using Microsoft.Extensions.Options;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models;
using smartFunds.Data.Repositories.Generic;

namespace smartFunds.Data.Repositories
{
    public interface IUserFundRepository : IRepository<UserFund>
    {
        
    }

    public class UserFundRepository : GenericRepository<UserFund>, IUserFundRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public UserFundRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }
    }
}
