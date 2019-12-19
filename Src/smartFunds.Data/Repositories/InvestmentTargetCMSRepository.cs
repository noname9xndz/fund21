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
    public interface IInvestmentTargetCMSRepository : IRepository<InvestmentTargetSetting>
    {
        List<InvestmentTargetSetting> GetAll();
        IEnumerable<InvestmentTargetSetting> GetInvestCMSModels(int[] modelIds);
    }
    public class InvestmentTargetCMSRepository : GenericRepository<InvestmentTargetSetting> , IInvestmentTargetCMSRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public InvestmentTargetCMSRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        public List<InvestmentTargetSetting> GetAll()
        {
            try
            {
                var listPortfolio = _smartFundsDbContext.Portfolio.ToList();
                foreach (var item in listPortfolio)
                {
                    if(!_smartFundsDbContext.InvestmentTargetSettings.Any(x => x.Portfolio.Id == item.Id))
                    {
                        var model = new InvestmentTargetSetting();
                        model.Portfolio = item;
                        _smartFundsDbContext.InvestmentTargetSettings.AddAsync(model);
                        _smartFundsDbContext.SaveChangesAsync();
                    }
                }
                return _smartFundsDbContext.InvestmentTargetSettings.ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<InvestmentTargetSetting> GetInvestCMSModels(int[] modelIds)
        {
            return _smartFundsDbContext.InvestmentTargetSettings.Where(m => modelIds.Contains(m.Id)).ToList();
        }
    }
}
