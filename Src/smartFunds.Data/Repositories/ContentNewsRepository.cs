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
    public interface IContentNewsRepository : IRepository<ContentNews>
    {
        IEnumerable<ContentNews> GetListContentNews(int[] newsIds);
    }

    public class ContentNewsRepository : GenericRepository<ContentNews>, IContentNewsRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public ContentNewsRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        public IEnumerable<ContentNews> GetListContentNews(int[] newsIds)
        {
            List<ContentNews> faqs = new List<ContentNews>();
            foreach (int newsId in newsIds)
            {
                ContentNews news = _smartFundsDbContext.ContentNews.Where(m => m.Id == newsId).FirstOrDefault();
                faqs.Add(news);
            }
            return faqs;
        }
    }
}
