using Microsoft.Extensions.Options;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models;
using smartFunds.Data.Repositories.Generic;

namespace smartFunds.Data.Repositories
{
    public interface IFAQRepository : IRepository<FAQ>
    {
    }

    public class FAQRepository : GenericRepository<FAQ>, IFAQRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public FAQRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }
    }
}
