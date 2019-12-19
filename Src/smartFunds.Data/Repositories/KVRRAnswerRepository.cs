using Microsoft.Extensions.Options;
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
    public interface IKVRRAnswerRepository : IRepository<KVRRAnswer>
    {
        IEnumerable<KVRRAnswer> GetAnswersByQuestionId(int id);
    }

    public class KVRRAnswerRepository : GenericRepository<KVRRAnswer>, IKVRRAnswerRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public KVRRAnswerRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        public IEnumerable<KVRRAnswer> GetAnswersByQuestionId(int id)
        {
            try
            {
                return _smartFundsDbContext.KvrrAnswer?.Where(a => a.KVRRQuestion.Id == id).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
