using smartFunds.Caching.AutoComplete;
using smartFunds.Common.Data.Repositories;
using smartFunds.Common.Options;
using smartFunds.Data.Extensions;
using smartFunds.Data.Models;
using smartFunds.Data.Repositories.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Data.Repositories
{
    public interface IEventRepository : IRepository<Event> {
        Task AddOrUpdate(Event entity);
        Task<bool> DeleteEvent(int eventId);
        Task<Event> GetEventByLocalityIdAndDate(int localityId, DateTime eventDate);
    }
    public class EventRepository : GenericPersistentTrackedRepository<Event>, IEventRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;
        public EventRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider, IHttpContextAccessor httpContextAccessor) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider, httpContextAccessor)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        /// <summary>
        /// Add or Update event and event sublocalities
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task AddOrUpdate(Event entity)
        {
            var @event = await GetAsync(x => x.MainLocalityId == entity.MainLocalityId && x.EventDate.Date == entity.EventDate.Date, "EventSublocalities");

            if (@event == null)
            {
                Add(entity);
                return;
            }

            var currentSublocalityIds = @event.EventSublocalities?.Select(x => x.SublocalityId).ToList() ?? new List<int>();
            var inputSublocalityIds = entity.EventSublocalities.Select(x => x.SublocalityId).ToList();

            var toAddSublocalityIds = inputSublocalityIds.Except(currentSublocalityIds).ToList();
            var toDeleteSublocalityIds = currentSublocalityIds.Except(inputSublocalityIds).ToList();

            // Return if no change 
            if (!toAddSublocalityIds.Any() && !toDeleteSublocalityIds.Any())
            {
                return;
            }

            if (@event.EventSublocalities != null && toDeleteSublocalityIds.Any())
            {
                foreach (var item in @event.EventSublocalities.Where(x => toDeleteSublocalityIds.Contains(x.SublocalityId)).ToList())
                {
                    // Soft delete
                    _smartFundsDbContext.SoftDelete(item, CurrentUser);
                }
            }

            if (toAddSublocalityIds.Any())
            {
                // Add new interchange locality
                _smartFundsDbContext.Set<EventSublocality>().AddRange(toAddSublocalityIds.Select(x => new EventSublocality
                {
                    SublocalityId = x,
                    EventId = @event.Id,
                    LastUpdatedBy = CurrentUser,
                    DateLastUpdated = DateTime.Now,
                    DeletedAt = "NA"
                }).ToList());

                _smartFundsDbContext.SaveChanges();
            }
        }

        /// <summary>
        /// Soft Delete event and Event sublocalities
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteEvent(int eventId)
        {
            var @event = await GetAsync(x => x.Id == eventId, "EventSublocalities");
            if (@event == null) return false;

            Delete(@event);
            if (@event.EventSublocalities != null && @event.EventSublocalities.Any())
            {
                foreach (var item in @event.EventSublocalities)
                {
                    // Soft delete
                    _smartFundsDbContext.SoftDelete(item, CurrentUser);
                }
            }

            // Save change
            _smartFundsDbContext.SaveChanges();

            return true;
        }

        /// <summary>
        /// Get event by localityId and eventDate
        /// </summary>
        /// <param name="localityId"></param>
        /// <param name="eventDate"></param>
        /// <returns></returns>
        public async Task<Event> GetEventByLocalityIdAndDate(int localityId, DateTime eventDate)
        {
            // Get Event
            var @event = await GetAsync(x => x.MainLocalityId == localityId && x.EventDate.Date == eventDate.Date,
                "Country,Locality,EventSublocalities,EventSublocalities.Sublocality");

            return @event;
        }
    }
}
