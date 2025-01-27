﻿using Microsoft.Extensions.Options;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models;
using smartFunds.Data.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace smartFunds.Data.Repositories
{
    public interface IKVRRQuestionRepository : IRepository<KVRRQuestion>
    {
        IEnumerable<KVRRQuestion> GetDefineKVRRQuestions();
        KVRRQuestion GetKVRRQuestionById(int id);
    }

    public class KVRRQuestionRepository : GenericRepository<KVRRQuestion>, IKVRRQuestionRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public KVRRQuestionRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        public IEnumerable<KVRRQuestion> GetDefineKVRRQuestions()
        {
            try
            {
                return _smartFundsDbContext.KvrrQuestion?.Include(i => i.KVRRAnswers)?.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public KVRRQuestion GetKVRRQuestionById(int id)
        {
            try
            {
                return  _smartFundsDbContext.KvrrQuestion?.Include(i => i.KVRRAnswers)?.FirstOrDefault(q => q.Id == id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
