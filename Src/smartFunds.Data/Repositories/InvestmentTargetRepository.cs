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

namespace smartFunds.Data.Repositories
{
    public interface IInvestmentTargetRepository : IRepository<InvestmentTarget>
    {
    }

    public class InvestmentTargetRepository : GenericPersistentTrackedRepository<InvestmentTarget>, IInvestmentTargetRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public InvestmentTargetRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider, IHttpContextAccessor httpContextAccessor) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider, httpContextAccessor)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }


    }
}
