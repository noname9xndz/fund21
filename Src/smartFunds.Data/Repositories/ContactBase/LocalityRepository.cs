using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models.Contactbase;
using smartFunds.Data.Repositories.Generic;
using Microsoft.Extensions.Options;

namespace smartFunds.Data.Repositories.ContactBase
{
    public interface ILocalityRepository : IReadOnlyRepository<Locality> {
    }
    public class LocalityRepository : ReadOnlyGenericRepository<Locality>, ILocalityRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;
        public LocalityRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base (smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }
    }
}
