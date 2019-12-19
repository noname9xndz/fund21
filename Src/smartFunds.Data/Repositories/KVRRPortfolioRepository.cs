using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models;
using smartFunds.Data.Repositories.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using smartFunds.Data.Extensions;

namespace smartFunds.Data.Repositories
{
    public interface IKVRRPortfolioRepository : IRepository<KVRRPortfolio>
    {
        
    }

    public class KVRRPortfolioRepository : GenericRepository<KVRRPortfolio>, IKVRRPortfolioRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public KVRRPortfolioRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        
    }
}
