using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models.Contactbase;
using smartFunds.Data.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace smartFunds.Data.Repositories.ContactBase
{
    public interface ICountryRepository : IReadOnlyRepository<Country> {
        Task<ICollection<Country>> GetMemberCountriesAsync(Member member, bool isAdmin, bool issmartFundsAdmin);
        Task<ICollection<Country>> GetAdditionalCountry(string countryCode);
    }
    public class CountryRepository : ReadOnlyGenericRepository<Country>, ICountryRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;
        public CountryRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base (smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        public async Task<ICollection<Country>> GetMemberCountriesAsync(Member member, bool isAdmin, bool issmartFundsAdmin)
        {
            // Filter country base on user's role
            var countryData =
            (isAdmin ? await GetAllAsync("Region,Localities,Localities.Sublocalities")
                : issmartFundsAdmin ? await FindByAsync(x => x.RegionId == member.RegionId, "Region,Localities,Localities.Sublocalities")
                    : await FindByAsync(x => x.Code == member.CountryCode, "Region,Localities,Localities.Sublocalities")).ToList();

            // smartFunds coordinartor can only get his locality
            if (!isAdmin && !issmartFundsAdmin && countryData.Any())
            {
                countryData = countryData.Take(1).ToList();
                countryData[0].Localities = countryData[0].Localities?.Where(x => x.Id == member.LocalityId).ToList();
            }

            return countryData;
        }

        /// <summary>
        /// Get additional country
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        public async Task<ICollection<Country>> GetAdditionalCountry(string countryCode)
        {
            var query = from c1 in _smartFundsDbContext.Countries
                from c2 in _smartFundsDbContext.Countries
                where c1.RegionId == c2.RegionId && c2.Code == countryCode
                where c1.Code != countryCode
                select c1;
            query = BuildIncludeQuery(query.AsQueryable().AsNoTracking(), "Region,Localities,Localities.Sublocalities");
            return await Task.FromResult(query.ToList());
        }
    }
}
