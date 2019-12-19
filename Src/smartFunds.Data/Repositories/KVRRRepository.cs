using System.Collections.Generic;
using System.Linq;
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
    public interface IKVRRRepository : IRepository<KVRR>
    {
        IEnumerable<KVRR> GetAllKVRR();
        KVRR GetKVRRById(int id);
    }

    public class KVRRRepository : GenericPersistentTrackedRepository<KVRR>, IKVRRRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;

        public KVRRRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider, IHttpContextAccessor httpContextAccessor) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider, httpContextAccessor)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        public IEnumerable<KVRR> GetAllKVRR()
        {
            try
            {
                return _smartFundsDbContext.Kvrr?.Include(p => p.KVRRPortfolios)?.Select(PopulateKVRR)?.ToList();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public KVRR GetKVRRById(int id)
        {
            try
            {
                return _smartFundsDbContext.Kvrr?.Include(k => k.KVRRPortfolios)?.Select(PopulateKVRR)?.FirstOrDefault(p => p.Id == id);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private KVRR PopulateKVRR(KVRR kvrr)
        {
            try
            {
                if (kvrr == null) return null;
                kvrr.KVRRPortfolios = kvrr.KVRRPortfolios.Select(z => new KVRRPortfolio
                {
                    KVRRId = z.KVRRId,
                    KVRR = z.KVRR,
                    PortfolioId = z.PortfolioId,
                    Portfolio = z.Portfolio ?? _smartFundsDbContext.Portfolio.FirstOrDefault(x => x.Id == z.PortfolioId)
                }).ToList();

                return kvrr;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}
