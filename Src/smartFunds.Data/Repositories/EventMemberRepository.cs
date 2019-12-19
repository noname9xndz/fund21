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
    public interface IEventGuestRepository : IRepository<EventGuest>
    {
        Task<List<EventGuest>> GetEventMembersBySublocalityId(List<int> sublocalityIds, int eventId);
        Task<List<int>> GetEventSublocalityIds(int eventId);
        Task<List<HouseholdConsecMeal>> GetAllEventHostByHousehold(List<int> householdIds, DateTime eventDate);
    }
    public class EventGuestRepository : GenericPersistentTrackedRepository<EventGuest>, IEventGuestRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;
        public EventGuestRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider, IHttpContextAccessor httpContextAccessor) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider, httpContextAccessor)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        /// <summary>
        /// Get all event member id who will be deleted
        /// </summary>
        /// <param name="sublocalityIds"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<List<EventGuest>> GetEventMembersBySublocalityId(List<int> sublocalityIds, int eventId)
        {
            var eventMembers = from e in _smartFundsDbContext.EventGuests
                               join m in _smartFundsDbContext.Members on e.MemberId equals m.Id
                               where m.SublocalityId != null && sublocalityIds.Contains(m.SublocalityId.Value) && e.EventId == eventId
                               select e;

            return await Task.FromResult(eventMembers.ToList());
        }

        /// <summary>
        /// Get all sublocalityIds of event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetEventSublocalityIds(int eventId)
        {
            var sublocalityIds = from e in _smartFundsDbContext.EventGuests
                                 join m in _smartFundsDbContext.Members on e.MemberId equals m.Id
                                 where e.EventId == eventId
                                 select m.SublocalityId.GetValueOrDefault();

            return await Task.FromResult(sublocalityIds.Distinct().ToList());
        }

        /// <summary>
        /// Get all event which household host in the pass
        /// </summary>
        /// <param name="householdIds"></param>
        /// <param name="eventDate"></param>
        /// <returns></returns>
        public async Task<List<HouseholdConsecMeal>> GetAllEventHostByHousehold(List<int> householdIds, DateTime eventDate)
        {
            var query = from r in(from h in _smartFundsDbContext.EventHosts
                        join e in _smartFundsDbContext.Events on h.EventId equals e.Id
                        where householdIds.Contains(h.HouseholderId) && e.EventDate < eventDate
                        select new
                        {
                            h.HouseholderId,
                            e.EventDate
                        })
                        group r by new
                        {
                            r.HouseholderId,
                            r.EventDate
                        } into tempR
                        select new HouseholdConsecMeal
                        {
                            HouseholderId = tempR.Key.HouseholderId,
                            EventDate = tempR.Key.EventDate
                        };

            return await Task.FromResult(query.ToList());
        }
    }
}
