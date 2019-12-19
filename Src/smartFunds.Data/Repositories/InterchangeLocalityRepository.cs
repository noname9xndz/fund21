using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models;
using smartFunds.Data.Repositories.Generic;
using Microsoft.Extensions.Options;

namespace smartFunds.Data.Repositories
{
    public interface IInterchangeLocalityRepository : IRepository<InterchangeLocality> {
    }
    public class InterchangeLocalityRepository : GenericRepository<InterchangeLocality>, IInterchangeLocalityRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;
        public InterchangeLocalityRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base (smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }
    }
}
