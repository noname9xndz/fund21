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
    public interface IWithdrawalFeeRepository : IRepository<WithdrawalFee>
    {
    }

    public class WithdrawalFeeRepository : GenericRepository<WithdrawalFee>, IWithdrawalFeeRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public WithdrawalFeeRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }


    }
}
