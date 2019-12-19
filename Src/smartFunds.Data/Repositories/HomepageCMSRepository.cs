using Microsoft.Extensions.Options;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models;
using smartFunds.Data.Repositories.Generic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smartFunds.Data.Repositories
{
    public interface IHomepageCMSRepository : IRepository<HomepageCMS>
    {
        List<HomepageCMS> GetAllHomepageConfiguration();
        HomepageCMS GetDataByCategory(int category, string typeUpload = "");
    }
    public class HomepageCMSRepository : GenericRepository<HomepageCMS>, IHomepageCMSRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;
        public HomepageCMSRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
           , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        public List<HomepageCMS> GetAllHomepageConfiguration()
        {
            try
            {
                return _smartFundsDbContext.HomepageConfigurations.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public HomepageCMS GetDataByCategory(int category, string typeUpload = "")
        {
            try
            {
                if(typeUpload != "")
                {
                    return _smartFundsDbContext.HomepageConfigurations.FirstOrDefault(h => int.Parse(h.Category) == category);
                }
                else
                {
                    return _smartFundsDbContext.HomepageConfigurations.FirstOrDefault(h => int.Parse(h.Category) == category &&
                    h.ImageName.Contains("_mobile"));
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
