using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models;
using smartFunds.Data.Repositories.Generic;
using Microsoft.Extensions.Options;

namespace smartFunds.Data.Repositories
{
    public interface ISettingRepository : IRepository<Setting> {
    }
    public class SettingRepository : GenericRepository<Setting>, ISettingRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;
        public SettingRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base (smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }
    }
}
