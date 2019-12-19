using System;
using AutoMapper;
using smartFunds.Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using smartFunds.Data.UnitOfWork;
using smartFunds.Common.Exceptions;
using Event = smartFunds.Service.Models.Event;
using Member = smartFunds.Data.Models.Contactbase.Member;

namespace smartFunds.Service.Services
{
    public interface IEventService
    {
        Task<Event> GetEventByLocalityIdAndDate(int localityId, DateTime eventDate);
        Task<Event> AddOrUpdateEventAsync(Event eventInput);
        Task<bool> DeleteEvent(int eventId);
        Task<List<DateTime>> GetAllEventsInYear(int localityId, int year);
    }
    public class EventService : IEventService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        #region Contructor

        public EventService(IMapper mapper = null, IUnitOfWork unitOfWork = null)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Get event by localityId and eventdate
        /// </summary>
        /// <param name="localityId"></param>D:\Project\smartFunds\Src\smartFunds.Data.ContactBase\Models\PhotoLink.cs
        /// <param name="eventDate"></param>
        /// <returns></returns>
        public async Task<Event> GetEventByLocalityIdAndDate(int localityId, DateTime eventDate)
        {
            // Get Event
            var @event = await _unitOfWork.EventRepository.GetEventByLocalityIdAndDate(localityId, eventDate);
            if (@event == null) return null;

            var result = _mapper.Map<Event>(@event);

            // Get locality information
            var localitiyIds = result.Sublocalities.Select(x => x.LocalityId).Distinct();
            var localities = await _unitOfWork.LocalityRepository.FindByAsync(x => localitiyIds.Contains(x.Id), "Sublocalities");
            var localityData = _mapper.Map<List<Locality>>(localities);

            foreach (var resultSublocality in result.Sublocalities)
            {
                resultSublocality.Locality = localityData.FirstOrDefault(x => x.Id == resultSublocality.LocalityId);
            }

            // Order sublocality
            result.Sublocalities = result.Sublocalities.OrderBy(x => x.Name).ToList();

            var region = (await _unitOfWork.CountryRepository.FindByAsync(x => x.Code == @event.CountryCode, "Region")).Select(x => x.Region).FirstOrDefault();
            result.Region = _mapper.Map<Region>(region);


            return result;
        }

        /// <summary>
        /// Add or Update event
        /// </summary>
        /// <param name="eventInput"></param>
        /// <returns></returns>
        public async Task<Event> AddOrUpdateEventAsync(Event eventInput)
        {
            if (eventInput == null)
            {
                throw new MissingParameterException();
            }

            eventInput.EventDate = eventInput.EventDate.Date;

            // Add event
            await _unitOfWork.EventRepository.AddOrUpdate(_mapper.Map<Data.Models.Event>(eventInput, opt => opt.Items.Add("eventId", eventInput.Id)));
            await _unitOfWork.SaveChangesAsync();

            var @event = await _unitOfWork.EventRepository.GetEventByLocalityIdAndDate(eventInput.MainLocalityId, eventInput.EventDate.Date);
            if (@event == null) return null;

            // Init event member
            await InitEventMember(eventInput, @event.Id);

            var result = _mapper.Map<Event>(@event);
            // Get locality information
            var localitiyIds = result.Sublocalities.Select(x => x.LocalityId).Distinct();
            var localities = await _unitOfWork.LocalityRepository.FindByAsync(x => localitiyIds.Contains(x.Id), "Sublocalities");
            var localityData = _mapper.Map<List<Locality>>(localities);

            foreach (var resultSublocality in result.Sublocalities)
            {
                resultSublocality.Locality = localityData.FirstOrDefault(x => x.Id == resultSublocality.LocalityId);
            }

            // Order sublocality
            result.Sublocalities = result.Sublocalities.OrderBy(x => x.Name).ToList();

            return result;
        }

        /// <summary>
        /// Delete a event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteEvent(int eventId)
        {
            return await _unitOfWork.EventRepository.DeleteEvent(eventId);
        }

        /// <summary>
        /// Get all events in one year
        /// </summary>
        /// <param name="localityId"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public async Task<List<DateTime>> GetAllEventsInYear(int localityId, int year)
        {
            // Get Events
            var items = await _unitOfWork.EventRepository.FindByAsync(x => x.MainLocalityId == localityId && x.EventDate.Year == year);
            var results = items?.Select(x => x.EventDate).Distinct().ToList();

            return results;
        }


        /// <summary>
        /// Init event member
        /// </summary>
        /// <param name="eventInput"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task InitEventMember(Event eventInput, int eventId)
        {
            // Get host sublocalities and guest sublocalities
            var mainSublocalityIds = (await _unitOfWork.SublocalityRepository.FindByAsync(x => x.LocalityId == eventInput.MainLocalityId)).Select(x => x.Id).ToList();
            var hostSublocalityIds = eventInput.Sublocalities.Where(x => mainSublocalityIds.Contains(x.Id)).Select(x => x.Id).ToList();
            var guestSublocalityIds = eventInput.Sublocalities.Where(x => !mainSublocalityIds.Contains(x.Id)).Select(x => x.Id).ToList();

            // Add host and event host
            await _unitOfWork.HostRepository.AddHost(hostSublocalityIds);
            await _unitOfWork.SaveChangesAsync();

            var currentSublocalityIds = await _unitOfWork.EventGuestRepository.GetEventSublocalityIds(eventId);
            var currentHostSublocalityIds = await _unitOfWork.EventHostRepository.GetEventSublocalityIds(eventId);
            if(currentHostSublocalityIds != null) currentSublocalityIds.AddRange(currentHostSublocalityIds);

            var inputSublocalityIds = eventInput.Sublocalities.Select(x => x.Id).ToList();

            // Delete event member who not belong to event
            var toDeleteSublocalityIds = currentSublocalityIds.Except(inputSublocalityIds).ToList();
            await DeleteEventMember(toDeleteSublocalityIds, eventId);

            // Add new event member
            var toAddSublocalityIds = inputSublocalityIds.Except(currentSublocalityIds).ToList();
            var hostSublocalities = toAddSublocalityIds.Where(x => hostSublocalityIds.Contains(x)).ToList();
            var guestSublocalities = toAddSublocalityIds.Where(x => guestSublocalityIds.Contains(x)).ToList();
            await AddEventMember(hostSublocalities, guestSublocalities, eventId);

            await _unitOfWork.SaveChangesAsync();
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Delete event member
        /// </summary>
        /// <param name="sublocalityIds"></param>
        /// <param name="eventId"></param>
        private async Task DeleteEventMember(List<int> sublocalityIds, int eventId)
        {
            var deletedMembers = await _unitOfWork.EventGuestRepository.GetEventMembersBySublocalityId(sublocalityIds, eventId);
            if (deletedMembers == null || !deletedMembers.Any()) return;
            _unitOfWork.EventGuestRepository.BulkDelete(deletedMembers);

            var deletedHosts = await _unitOfWork.EventHostRepository.GetEventHostsBySublocalityId(sublocalityIds, eventId);
            if (deletedHosts == null || !deletedHosts.Any()) return;
            _unitOfWork.EventHostRepository.BulkDelete(deletedHosts);

        }

        /// <summary>
        /// Add event member
        /// </summary>
        /// <param name="hostSublocalityIds"></param>
        /// <param name="guestSublocalityIds"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        private async Task AddEventMember(List<int> hostSublocalityIds, List<int> guestSublocalityIds, int eventId)
        {
            var memberHosts = await _unitOfWork.MemberRepository.FindByAsync(x => x.SublocalityId != null && hostSublocalityIds.Contains(x.SublocalityId.Value) && x.IsHidden == false && x.IsDeceased == false);

            // Get householdId to check if all members of a Household >= 80
            var householdIds = memberHosts.Select(x => x.HouseholderId).Distinct().ToList();
            var hosts = await _unitOfWork.HostRepository.FindByAsync(x => householdIds.Contains(x.HouseholderId));
            var eventHostMembers = new List<Member>();
            var oldHouseholderIds = new List<int>();
            foreach (var householdId in householdIds)
            {
                if(householdId == null) continue;
                if (memberHosts.Any(x => x.HouseholderId == householdId && x.Age < 80))
                {
                    var hostHousehold = memberHosts.FirstOrDefault(x => x.HouseholderId == householdId);
                    if(hostHousehold == null) continue;
                    eventHostMembers.Add(hostHousehold);
                }
                else
                {
                    oldHouseholderIds.Add(householdId.Value);
                }
            }
            if (eventHostMembers.Any())
            {
                var eventHosts = new List<Data.Models.EventHost>();
                foreach (var hostMember in eventHostMembers)
                {
                    var host = hosts.FirstOrDefault(x => x.HouseholderId == hostMember.HouseholderId);
                    if (host == null) continue;

                    eventHosts.Add(new Data.Models.EventHost
                    {
                        HouseholderId = hostMember.HouseholderId ?? 0,
                        HostId = host.Id,
                        CP = host.DefaultCP,
                        SCP = host.DefaultSCP,
                        EventId = eventId
                    });
                }

                _unitOfWork.EventHostRepository.BulkInsert(eventHosts);
            }

            var guest = await _unitOfWork.MemberRepository.FindByAsync(x => ((x.SublocalityId != null && guestSublocalityIds.Contains(x.SublocalityId.Value)) || oldHouseholderIds.Contains(x.HouseholderId ?? 0)) && x.IsHidden == false && x.IsDeceased == false);
            if (guest.Any())
            {
                _unitOfWork.EventGuestRepository.BulkInsert(guest.Select(x => new Data.Models.EventGuest
                {
                    EventId = eventId,
                    MemberId = x.Id,
                    HouseholderId = x.HouseholderId.GetValueOrDefault(),
                    IsAway = false,
                    IsToBeAssigned = false
                }).ToList());
            }
        }

        #endregion
    }
}
