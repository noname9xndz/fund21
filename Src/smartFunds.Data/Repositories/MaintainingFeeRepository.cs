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

namespace smartFunds.Data.Repositories
{
    public interface IMaintainingFeeRepository : IRepository<MaintainingFee>
    {
        List<MaintainingFee> GetMaintainingFeeDefault();
        List<MaintainingFee> GetMaintainingFeeByIds(int[] ids);
        MaintainingFee GetMaintainingFeeById(int id);
    }
    public class MaintainingFeeRepository : GenericRepository<MaintainingFee>, IMaintainingFeeRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public MaintainingFeeRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }


        public MaintainingFee GetMaintainingFeeById(int id)
        {
            try
            {
                return _smartFundsDbContext.MaintainingFees?.FirstOrDefault(p => p.Id == id);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public List<MaintainingFee> GetMaintainingFeeByIds(int[] ids)
        {
            try
            {
                return _smartFundsDbContext.MaintainingFees?.Where(x => ids.Contains(x.Id))?.ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public List<MaintainingFee> GetMaintainingFeeDefault()
        {
            try
            {
                return _smartFundsDbContext.MaintainingFees.ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

    }
}
