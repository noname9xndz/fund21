using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models.Contactbase;
using smartFunds.Data.Repositories.Generic;
using Microsoft.Extensions.Options;

namespace smartFunds.Data.Repositories.ContactBase
{
    public interface ISublocalityRepository : IReadOnlyRepository<Sublocality> {
    }
    public class SublocalityRepository : ReadOnlyGenericRepository<Sublocality>, ISublocalityRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;
        public SublocalityRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base (smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }
    }
}
