using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models.HangFire;
using smartFunds.Data.Repositories.Generic;
using Microsoft.Extensions.Options;

namespace smartFunds.Data.Repositories.HangFire
{
    public interface IJobRepository : IRepository<Job>
    {

    }

    public class JobRepository : GenericRepository<Job>, IJobRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;
        public JobRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }
    }
}
