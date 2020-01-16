using Microsoft.Extensions.Options;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models;
using smartFunds.Data.Repositories.Generic;
using System.Collections.Generic;
using System.Linq;

namespace smartFunds.Data.Repositories
{

    public interface ICateNewsRepository : IRepository<CateNews>
    {
        IEnumerable<CateNews> GetListCateNews(int[] cateNewsIds);
    }

    public class CateNewsRepository : GenericRepository<CateNews>, ICateNewsRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public CateNewsRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        public IEnumerable<CateNews> GetListCateNews(int[] cateNewsIds)
        {
            List<CateNews> cates = new List<CateNews>();
            foreach (int cateId in cateNewsIds)
            {
                CateNews cate = _smartFundsDbContext.CateNews.Where(m => m.Id == cateId).FirstOrDefault();
                cates.Add(cate);
            }
            return cates;
        }
    }
}
