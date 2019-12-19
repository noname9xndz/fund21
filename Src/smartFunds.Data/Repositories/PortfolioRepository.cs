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
    public interface IPortfolioRepository : IRepository<Portfolio>
    {
        IEnumerable<Portfolio> GetAllPortfolio();
        Portfolio GetPortfolioById(int id);
    }

    public class PortfolioRepository : GenericPersistentTrackedRepository<Portfolio>, IPortfolioRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public PortfolioRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider, IHttpContextAccessor httpContextAccessor) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider, httpContextAccessor)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }


        public IEnumerable<Portfolio> GetAllPortfolio()
        {
            try
            {
                return _smartFundsDbContext.Portfolio?.Include(p => p.KVRRPortfolios)?.Select(PopulatePortfolio)?.ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private Portfolio PopulatePortfolio(Portfolio p)
        {
            try
            {
                if (p == null) return null;
                p.KVRRPortfolios = p.KVRRPortfolios.Select(z => new KVRRPortfolio
                {
                    KVRRId = z.KVRRId,
                    KVRR = z.KVRR ?? _smartFundsDbContext.Kvrr.FirstOrDefault(x => x.Id == z.KVRRId),
                    PortfolioId = z.PortfolioId,
                    Portfolio = z.Portfolio
                }).ToList();
                p.PortfolioFunds = p.PortfolioFunds.Select(z => new PortfolioFund
                {
                    PortfolioId = z.PortfolioId,
                    Portfolio = z.Portfolio,
                    FundId = z.FundId,
                    Fund = z.Fund ?? _smartFundsDbContext.Fund.FirstOrDefault(x => x.Id == z.FundId),
                    FundPercent = z.FundPercent,
                    FundPercentNew = z.FundPercentNew
                }).Where(x => x.PortfolioId == p.Id).ToList();

                return p;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public Portfolio GetPortfolioById(int id)
        {
            try
            {
                return _smartFundsDbContext.Portfolio?.Include(p => p.KVRRPortfolios).Include(p => p.PortfolioFunds)?.Select(PopulatePortfolio)?.FirstOrDefault(p => p.Id == id);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
