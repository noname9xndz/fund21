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
    public interface IFundPurchaseFeeRepository : IRepository<FundPurchaseFee>
    {
    }

    public class FundPurchaseFeeRepository : GenericRepository<FundPurchaseFee>, IFundPurchaseFeeRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public FundPurchaseFeeRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }


    }
}
