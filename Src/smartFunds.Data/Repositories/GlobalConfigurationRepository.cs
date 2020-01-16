﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models;
using smartFunds.Data.Repositories.Generic;

namespace smartFunds.Data.Repositories
{

    public interface IGlobalConfigurationRepository : IRepository<GlobalConfiguration>
    {
    }

    public class GlobalConfigurationRepository : GenericRepository<GlobalConfiguration>, IGlobalConfigurationRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public GlobalConfigurationRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }


    }
}