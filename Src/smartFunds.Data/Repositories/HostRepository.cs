using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models;
using smartFunds.Data.Repositories.Generic;
using Microsoft.Extensions.Options;

namespace smartFunds.Data.Repositories
{
    public interface IHostRepository : IRepository<Host> {
        Task AddHost(List<int> hostSublocalityIds);
        Task<List<Host>> GetHostsInSublocality(List<int> hostSublocalityIds);
    }
    public class HostRepository : GenericRepository<Host>, IHostRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;
        public HostRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider) : base (smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        /// <summary>
        /// Add all information about event member when create event
        /// </summary>
        /// <param name="hostSublocalityIds"></param>
        /// <returns></returns>
        public async Task AddHost(List<int> hostSublocalityIds)
        {
            // Get all householder Id in sublocality
            var householderIds = _smartFundsDbContext.Members.Where(x => x.SublocalityId != null && hostSublocalityIds.Contains(x.SublocalityId.Value) && x.IsHouseholder).Select(x => x.HouseholderId.Value).ToList();

            // Get all exits householder
            var oldHouseholderIds = (await FindByAsync(x => householderIds.Contains(x.HouseholderId))).Select(x => x.HouseholderId).ToList();

            // Get all householder need to be added
            var newHouseholderids = householderIds.Except(oldHouseholderIds).ToList();

            // Insert new host
            if (newHouseholderids.Any())
            {
                BulkInsert(newHouseholderids.Select(x => new Host
                {
                    HouseholderId = x,
                    DefaultCP = 0,
                    DefaultSCP = 0
                }).ToList());
            }
        }

        /// <summary>
        /// Get host
        /// </summary>
        /// <param name="hostSublocalityIds"></param>
        /// <returns></returns>
        public async Task<List<Host>> GetHostsInSublocality(List<int> hostSublocalityIds)
        {
            // Get all host Id in sublocality
            var hosts = (from h in _smartFundsDbContext.Hosts
                join m in _smartFundsDbContext.Members on h.HouseholderId equals m.HouseholderId
                where m.SublocalityId != null && hostSublocalityIds.Contains(m.SublocalityId.Value) && m.IsHouseholder
                select h).ToList();

            return await Task.FromResult(hosts.ToList());
        }
    }
}
