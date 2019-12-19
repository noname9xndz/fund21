using System;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models;
using smartFunds.Data.Repositories.Generic;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using smartFunds.Data.Extensions;
using Microsoft.AspNetCore.Http;

namespace smartFunds.Data.Repositories
{
    public interface IInterchangeRepository : IRepository<Interchange> {
        Task AddOrUpdate(Interchange entity);
        Task<Interchange> GetInterchangeByLocality(int localityId);
    }
    public class InterchangeRepository : GenericPersistentTrackedRepository<Interchange>, IInterchangeRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;
        public InterchangeRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider, IHttpContextAccessor httpContextAccessor) : base (smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider, httpContextAccessor)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        /// <summary>
        /// Add or update interchange
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task AddOrUpdate(Interchange entity)
        {            
            var interchange = await GetAsync(x => x.MainLocalityId == entity.MainLocalityId, "InterchangeLocalities");

            if (interchange == null)
            {
                Add(entity);
                return;
            }

            var currentLocalityIds = interchange.InterchangeLocalities?.Select(x => x.LocalityId).ToList() ?? new List<int>();
            var inputLocalityIds = entity.InterchangeLocalities.Select(x => x.LocalityId).ToList();

            var toAddLocalityIds = inputLocalityIds.Except(currentLocalityIds).ToList();
            var toDeleteLocalityIds = currentLocalityIds.Except(inputLocalityIds).ToList();

            // Return if no change 
            if (!toAddLocalityIds.Any() && !toDeleteLocalityIds.Any() && string.Equals(interchange.EmailAddress, entity.EmailAddress))
            {
                return;
            }

            if (!string.Equals(interchange.EmailAddress, entity.EmailAddress))
            {
                interchange.EmailAddress = entity.EmailAddress;
                Update(interchange);
            }

            if (interchange.InterchangeLocalities != null && toDeleteLocalityIds.Any())
            {
                foreach (var item in interchange.InterchangeLocalities.Where(x => toDeleteLocalityIds.Contains(x.LocalityId)).ToList())
                {
                    // Soft delete
                    _smartFundsDbContext.SoftDelete(item, CurrentUser);
                }

            }

            if (toAddLocalityIds.Any())
            {
                // Add new interchange locality
                _smartFundsDbContext.Set<InterchangeLocality>().AddRange(toAddLocalityIds.Select(x => new InterchangeLocality
                {
                    LocalityId = x,
                    InterchangeId = interchange.Id,
                    LastUpdatedBy = CurrentUser,
                    DateLastUpdated = DateTime.Now,
                    DeletedAt = "NA"
                }).ToList());

                _smartFundsDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Get interchange by localityId
        /// </summary>
        /// <param name="localityId"></param>
        /// <returns></returns>
        public async Task<Interchange> GetInterchangeByLocality(int localityId)
        {
            // Get interchange information
            var interchange = await GetAsync(x => x.MainLocalityId == localityId,
                "Country,Locality,InterchangeLocalities,InterchangeLocalities.Locality,InterchangeLocalities.Locality.Sublocalities");

            return interchange;
        }
    }
}
