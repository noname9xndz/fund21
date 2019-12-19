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
    public interface IContactCMSRepository : IRepository<ContactCMS>
    {
        ContactCMS GetContactConfiguration();
    }
    public class ContactCMSRepository : GenericRepository<ContactCMS>, IContactCMSRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public ContactCMSRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        public ContactCMS GetContactConfiguration()
        {
            try
            {
                return _smartFundsDbContext.ContactConfigurations.FirstOrDefault();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
