using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Models;
using smartFunds.Data.Repositories.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace smartFunds.Data.Repositories
{
    public interface IEventHostRepository : IRepository<EventHost>
    {
        Task<List<HouseholdCapacity>> GetHouseholdCapacity(List<int> householdIds, int eventId);
        Task<List<EventHost>> GetEventHostsBySublocalityId(List<int> sublocalityIds, int eventId);
        Task<List<int>> GetEventSublocalityIds(int eventId);
        Task<List<EventHost>> GetAllFutureEventHost(int eventId, int houreholdId);
    }
    public class EventHostRepository : GenericPersistentTrackedRepository<EventHost>, IEventHostRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;
        public EventHostRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider, IHttpContextAccessor httpContextAccessor) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider, httpContextAccessor)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        /// <summary>
        /// Get household capacity
        /// </summary>
        /// <param name="householdIds"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<List<HouseholdCapacity>> GetHouseholdCapacity(List<int> householdIds, int eventId)
        {
            if (householdIds == null || !householdIds.Any()) return new List<HouseholdCapacity>();

            var householdCapacity = from h in _smartFundsDbContext.Hosts
                                    join e in _smartFundsDbContext.EventHosts on h.Id equals e.HostId
                                    where householdIds.Contains(h.HouseholderId) && e.EventId == eventId
                                    select new HouseholdCapacity
                                    {
                                        HouseholderId = h.HouseholderId,
                                        CP = e.CP,
                                        SCP = e.SCP,
                                        DefaultCP = h.DefaultCP,
                                        DefaultSCP = h.DefaultSCP
                                    };

            return await Task.FromResult(householdCapacity.ToList());
        }


        /// <summary>
        /// Get event host by sublocality id
        /// </summary>
        /// <param name="sublocalityIds"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<List<EventHost>> GetEventHostsBySublocalityId(List<int> sublocalityIds, int eventId)
        {
            var eventHosts = from e in _smartFundsDbContext.EventHosts
                             join m in _smartFundsDbContext.Members on e.HouseholderId equals m.HouseholderId
                             where m.SublocalityId != null && sublocalityIds.Contains(m.SublocalityId.Value) && m.IsHouseholder && e.EventId == eventId
                             select e;

            return await Task.FromResult(eventHosts.ToList());
        }

        /// <summary>
        /// Get all sublocalityIds of event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetEventSublocalityIds(int eventId)
        {
            var sublocalityIds = from e in _smartFundsDbContext.EventHosts
                                 join m in _smartFundsDbContext.Members on e.HouseholderId equals m.Id
                                 where e.EventId == eventId
                                 select m.SublocalityId.GetValueOrDefault();

            return await Task.FromResult(sublocalityIds.Distinct().ToList());
        }

        /// <summary>
        /// Get all future eventhost of household
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="householdId"></param>
        /// <returns></returns>
        public async Task<List<EventHost>> GetAllFutureEventHost(int eventId, int householdId)
        {
            var eventHosts = from h in _smartFundsDbContext.EventHosts
                             join e in _smartFundsDbContext.Events on h.EventId equals e.Id
                             where h.HouseholderId == householdId && e.Id != eventId && e.EventDate > DateTime.Now
                             select h;

            return await Task.FromResult(eventHosts.ToList());
        }
    }
}
