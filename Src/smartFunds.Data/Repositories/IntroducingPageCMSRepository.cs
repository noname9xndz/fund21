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

namespace smartFunds.Data.Repositories
{
    
    public interface IIntroducingPageCMSRepository : IRepository<GenericIntroducingSetting>
    {
        GenericIntroducingSetting GetSetting();
    }
    public class IntroducingPageCMSRepository : GenericRepository<GenericIntroducingSetting>, IIntroducingPageCMSRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public IntroducingPageCMSRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }
        

        public GenericIntroducingSetting GetSetting()
        {
            try
            {
                return _smartFundsDbContext.GenericIntroducingSettings.FirstOrDefault();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

    }
}
