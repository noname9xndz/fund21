using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models;
using smartFunds.Data.Repositories.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using smartFunds.Data.Extensions;

namespace smartFunds.Data.Repositories
{
    public interface IFundRepository : IRepository<Fund>
    {
        IEnumerable<Fund> GetAllFund();
        Fund GetFundById(int id);
        IEnumerable<Fund> GetFundsByIds(int[] ids);
    }

    public class FundRepository : GenericPersistentTrackedRepository<Fund>, IFundRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public FundRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider, IHttpContextAccessor httpContextAccessor) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider, httpContextAccessor)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }


        public IEnumerable<Fund> GetAllFund()
        {
            try
            {
                return _smartFundsDbContext.Fund?.Include(p => p.PortfolioFunds)?.Select(PopulateFund)?.ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private Fund PopulateFund(Fund p)
        {
            try
            {
                if (p == null) return null;
                p.PortfolioFunds = p.PortfolioFunds.Select(z => new PortfolioFund()
                {
                    PortfolioId = z.PortfolioId,
                    Portfolio = z.Portfolio ?? _smartFundsDbContext.Portfolio.FirstOrDefault(x => x.Id == z.PortfolioId),
                    FundId = z.FundId,
                    Fund = z.Fund ?? _smartFundsDbContext.Fund.FirstOrDefault(x => x.Id == z.FundId),
                    FundPercent = z.FundPercent ?? 0,
                    FundPercentNew = z.FundPercentNew ?? 0,
                    EditStatus = z.EditStatus
                }).ToList();

                return p;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public Fund GetFundById(int id)
        {
            try
            {
                return _smartFundsDbContext.Fund?.Include(p => p.PortfolioFunds)?.Select(PopulateFund)?.FirstOrDefault(p => p.Id == id);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<Fund> GetFundsByIds(int[] ids)
        {
            try
            {
                return _smartFundsDbContext.Fund?.Include(p => p.PortfolioFunds).Select(PopulateFund)?.Where(x => ids.Contains(x.Id))?.ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
