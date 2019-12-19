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
    public interface IFAQRepository : IRepository<FAQ>
    {
        IEnumerable<FAQ> GetFAQs(int[] faqIds);
    }

    public class FAQRepository : GenericRepository<FAQ>, IFAQRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public FAQRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        public IEnumerable<FAQ> GetFAQs(int[] faqIds)
        {
            List<FAQ> faqs = new List<FAQ>();
            foreach (int faqId in faqIds)
            {
                FAQ faq = _smartFundsDbContext.FAQs.Where(m => m.Id == faqId).FirstOrDefault();
                faqs.Add(faq);
            }
            return faqs;            
        }
    }
}
