using System.Collections.Generic;
using Microsoft.Extensions.Options;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models;
using smartFunds.Data.Repositories.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace smartFunds.Data.Repositories
{
    /*
     * Creator by PhuongNC
     */
    public interface ICustomerLevelRepository : IRepository<CustomerLevel>
    {
        IEnumerable<CustomerLevel> GetAllCustomerLevel();
        CustomerLevel GetCustomerLevelById(int id);
        IEnumerable<CustomerLevel> GetCustomerLevelsByIds(int[] ids);
    }

    public class CustomerLevelRepository : GenericPersistentTrackedRepository<CustomerLevel>, ICustomerLevelRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public CustomerLevelRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
    , IRedisAutoCompleteProvider redisAutoCompleteProvider, IHttpContextAccessor httpContextAccessor) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider, httpContextAccessor)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        public IEnumerable<CustomerLevel> GetAllCustomerLevel()
        {
            try
            {
                return _smartFundsDbContext.CustomerLevel.ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public CustomerLevel GetCustomerLevelById(int id)
        {
            try
            {
                return _smartFundsDbContext.CustomerLevel.FirstOrDefault(p => p.IDCustomerLevel == id);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<CustomerLevel> GetCustomerLevelsByIds(int[] ids)
        {
            try
            {
                return _smartFundsDbContext.CustomerLevel.Where(x => ids.Contains(x.IDCustomerLevel))?.ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
