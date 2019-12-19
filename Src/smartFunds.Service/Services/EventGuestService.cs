using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using smartFunds.Common;
using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using smartFunds.Service.Models;
using EventGuest = smartFunds.Service.Models.EventGuest;

namespace smartFunds.Service.Services
{
    public interface IEventGuestService
    {
        Task<EventGuest> GetEventGuest(int eventId);
        Task<MealStatistics> GetMealStatistics(int eventId);
        Task<AwayList> GetAwayMemberList(int eventId);
        Task UpdateEventMembers(EventMemberRole roleModel);
        Task AddEventMembers(List<EventGuestModel> model);
        Task AddEventHosts(MemberHost model);
        Task DeleteEventMember(List<int> eventMemberids);
       
    }
    public class EventGuestService : IEventGuestService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        #region Contructor

        public EventGuestService(IMapper mapper = null, IUnitOfWork unitOfWork = null)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Get all information of member who join event meal
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<EventGuest> GetEventGuest(int eventId)
        {
            var @event = await _unitOfWork.EventRepository.GetAsync(x => x.Id == eventId);
            if (@event == null) return null;

            // Get member in event and seperate by type
            var eventMembers = await _unitOfWork.EventGuestRepository.FindByAsync(x => x.EventId == eventId);
            var eventMemberIds = eventMembers.Select(x => x.MemberId);

            // Cook event memberid for guest
            var eventGuestHouseholdIds = eventMembers.Where(x => x.IsToBeAssigned == false && x.IsAway == false).Select(x => x.HouseholderId).Distinct().ToList();
            var eventguestMembers = eventMembers.Where(x => (x.IsToBeAssigned == false && x.IsAway == false) || (x.IsAway && eventGuestHouseholdIds.Contains(x.HouseholderId))).ToList();
            var guestMemberIds = eventguestMembers.Select(x => x.MemberId);
            var guestAwayIds = eventMembers.Where(x => x.IsAway && eventGuestHouseholdIds.Contains(x.HouseholderId)).Select(x => x.MemberId).ToList();

            // Cook event memberid for tba
            var eventTbaHouseholdIds = eventMembers.Where(x => x.IsToBeAssigned).Select(x => x.HouseholderId).Distinct().ToList();
            var eventTbaMembers = eventMembers.Where(x => x.IsToBeAssigned || (x.IsAway && eventTbaHouseholdIds.Contains(x.HouseholderId))).ToList();
            var tbaMemberIds = eventTbaMembers.Select(x => x.MemberId);
            var tbaAwayIds = eventMembers.Where(x => x.IsAway && eventTbaHouseholdIds.Contains(x.HouseholderId)).Select(x => x.MemberId).ToList();
            var householdOfEvents = await _unitOfWork.EventGuestRepository.GetAllEventHostByHousehold(eventTbaHouseholdIds, @event.EventDate);


            // Get member information and seperate by type
            var members = await _unitOfWork.MemberRepository.FindByAsync(x => eventMemberIds.Contains(x.Id) && x.IsHidden == false && x.IsDeceased == false, "Locality,Sublocality");
            var guestMembers = members.Where(x => guestMemberIds.Contains(x.Id)).ToList();
            var tobeAssignedMembers = members.Where(x => tbaMemberIds.Contains(x.Id)).ToList();

            // Populate data
            var guestSection = PopulateEventGuest(guestMembers, guestAwayIds, eventguestMembers);
            var toBeAssignedSection = PopulateEventTba(tobeAssignedMembers, tbaAwayIds, eventTbaMembers, eventGuestHouseholdIds, householdOfEvents, @event.EventDate);

            return new EventGuest
            {
                GuestSection = guestSection,
                ToBeAssignedSection = toBeAssignedSection
            };
        }

        /// <summary>
        /// Get acceptance criteria of event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<MealStatistics> GetMealStatistics(int eventId)
        {
            var @event = await _unitOfWork.EventRepository.GetAsync(x => x.Id == eventId);
            if (@event == null) return null;

            // Get member in event and seperate by type
            var eventMembers = await _unitOfWork.EventGuestRepository.FindByAsync(x => x.EventId == eventId);
            var eventHostHouseholdIds = (await _unitOfWork.EventHostRepository.FindByAsync(x => x.EventId == eventId)).Select(x => x.HouseholderId).Distinct().ToList();

            var totalMemberInHostHouseholds = (await _unitOfWork.MemberRepository.FindByAsync(x => eventHostHouseholdIds.Contains(x.HouseholderId.Value) && !x.IsDeceased && !x.IsHidden)).Count;
            var totalGuests = eventMembers.Count(x => !x.IsAway && !x.IsToBeAssigned);
            var totalPersonsTba = eventMembers.Count(x => x.IsToBeAssigned);

            var householdCapacity = await _unitOfWork.EventHostRepository.GetHouseholdCapacity(eventHostHouseholdIds, @event.Id);
            var hostHouseholdCapacity = householdCapacity.Where(x => eventHostHouseholdIds.Contains(x.HouseholderId)).ToList();
            var totalHouseholds = eventHostHouseholdIds.Count;

            var mealStatistics = new MealStatistics
            {
                TotalHouseholds = totalHouseholds,
                NoInHostHouseholds = totalMemberInHostHouseholds,
                TotalCP = hostHouseholdCapacity.Sum(x => x.CP),
                TotalSCP = hostHouseholdCapacity.Sum(x => x.SCP),
                TotalGuests = totalGuests,
                TotalPersonsTba = totalPersonsTba,
                AverageBreakSize = totalHouseholds == 0 ? 0 : Math.Ceiling((double)totalGuests / totalHouseholds),
                Discrepancy = totalGuests - hostHouseholdCapacity.Sum(x => x.CP)
            };

            return mealStatistics;
        }

        /// <summary>
        /// Get all away member in event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<AwayList> GetAwayMemberList(int eventId)
        {
            var eventMemberAways = await _unitOfWork.EventGuestRepository.FindByAsync(x => x.EventId == eventId && x.IsAway);
            var awayMemberIds = eventMemberAways.Select(x => x.MemberId);

            // Get member infor
            var members = await _unitOfWork.MemberRepository.FindByAsync(x => awayMemberIds.Contains(x.Id), "Locality,Sublocality");

            var awayMembers = new List<AwayMember>();
            foreach (var member in members)
            {
                var eventMember = eventMemberAways.FirstOrDefault(x => x.MemberId == member.Id);
                if (eventMember == null) continue;

                awayMembers.Add(new AwayMember
                {
                    EventGuestId = eventMember.Id,
                    MemberId = member.Id,
                    Title = member.Title,
                    Name = $"{member.FirstName} {member.LastName}",
                    HouseholderId = member.HouseholderId ?? 0,
                    Locality = member.LocalityName,
                    CountryCode = member.CountryCode,
                    LocalityId = member.LocalityId ?? 0,
                    SublocalityId = member.SublocalityId ?? 0,
                    SublocalityName = member.SublocalityName
                });
            }

            if (!awayMembers.Any()) return null;

            // Cook data for locality
            var localities = members.Select(x => x.Locality).GroupBy(x => x.Id).Select(x => x.First()).ToList();
            var sublocaloties = members.Select(x => x.Sublocality).GroupBy(x => x.Id).Select(x => x.First()).ToList();
            foreach (var locality in localities)
            {
                locality.Sublocalities = sublocaloties.Where(x => x.LocalityId == locality.Id).OrderBy(x => x.Name).ToList();
            }
            localities = localities.OrderBy(x => x.Name).ToList();

            var result = new AwayList
            {
                AwayMembers = awayMembers,
                Localities = _mapper.Map<List<Locality>>(localities)
            };

            return result;
        }

        /// <summary>
        /// Update event member
        /// </summary>
        /// <param name="roleModel"></param>
        /// <returns></returns>
        public async Task UpdateEventMembers(EventMemberRole roleModel)
        {
            if (roleModel.IsHost)
            {
                // Insert new host if need
                await AddHostAndEventHost(roleModel.HouseholderIds, roleModel.EventId);

                // Get member of household in event as guest/away/tba then delete
                var memberInEvent = await _unitOfWork.EventGuestRepository.FindByAsync(x => x.EventId == roleModel.EventId && roleModel.HouseholderIds.Contains(x.HouseholderId));
                _unitOfWork.EventGuestRepository.BulkDelete(memberInEvent);

                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                if (roleModel.HouseholderIds.Any())
                {
                    // Get all member in household and insert into eventmember
                    var members = await _unitOfWork.MemberRepository.FindByAsync(x => roleModel.HouseholderIds.Contains(x.HouseholderId ?? 0) && !x.IsHidden && !x.IsDeceased);
                    _unitOfWork.EventGuestRepository.BulkInsert(members.Select(x => new Data.Models.EventGuest
                    {
                        EventId = roleModel.EventId,
                        MemberId = x.Id,
                        HouseholderId = x.HouseholderId ?? 0,
                        IsAway = roleModel.IsAway,
                        IsToBeAssigned = roleModel.IsTba
                    }).ToList());

                    // Delete all household in event host
                    var eventHosts = await _unitOfWork.EventHostRepository.FindByAsync(x => x.EventId == roleModel.EventId && roleModel.HouseholderIds.Contains(x.HouseholderId));
                    _unitOfWork.EventHostRepository.BulkDelete(eventHosts);
                }
                else
                {
                    var eventMembers = await _unitOfWork.EventGuestRepository.FindByAsync(x => roleModel.EventGuestIds.Contains(x.Id));
                    if (eventMembers == null || !eventMembers.Any()) return;

                    foreach (var eventMember in eventMembers)
                    {
                        eventMember.IsAway = roleModel.IsAway;
                        eventMember.IsToBeAssigned = roleModel.IsTba;
                    }
                    // Update event members
                    _unitOfWork.EventGuestRepository.BulkUpdate(eventMembers);
                }

                await _unitOfWork.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Add new event member into event
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task AddEventMembers(List<EventGuestModel> model)
        {
            var eventMembers = _mapper.Map<List<Data.Models.EventGuest>>(model);
            if (eventMembers == null || !eventMembers.Any())
            {
                throw new MissingParameterException();
            }

            // Insert into EventGuest table
            _unitOfWork.EventGuestRepository.BulkInsert(eventMembers);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Add new event host
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task AddEventHosts(MemberHost model)
        {  
            var @event = await _unitOfWork.EventRepository.GetAsync(x => x.Id == model.EventId);
            if(@event == null) throw new MissingParameterException();

            await AddHostAndEventHost(model.HouseholderIds, model.EventId);

            var eventHostHouseholdAddedIds = (await _unitOfWork.EventHostRepository.FindByAsync(x => model.HouseholderIds.Contains(x.HouseholderId))).Select(x=>x.HouseholderId).ToList();
            var eventMemberInHostHouseholds = await _unitOfWork.EventGuestRepository.FindByAsync(x => eventHostHouseholdAddedIds.Contains(x.HouseholderId));
            _unitOfWork.EventGuestRepository.BulkDelete(eventMemberInHostHouseholds);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Delete event member from event
        /// </summary>
        /// <param name="eventMemberids"></param>
        /// <returns></returns>
        public async Task DeleteEventMember(List<int> eventMemberids)
        {
            // Get eventmember
            var eventMembers = await _unitOfWork.EventGuestRepository.FindByAsync(x => eventMemberids.Contains(x.Id));
            if (eventMembers == null || !eventMembers.Any()) throw new MissingParameterException();

            _unitOfWork.EventGuestRepository.BulkDelete(eventMembers);
            await _unitOfWork.SaveChangesAsync();
        }
        
        #endregion

        #region Private Method

        /// <summary>
        /// Populate data for guest section in Meal Set-up screen
        /// </summary>
        /// <param name="members"></param>
        /// <param name="awayMembers"></param>
        /// <param name="eventMemberIds"></param>
        /// <returns></returns>
        private MealSetupSectionModel PopulateEventGuest(List<Data.Models.Contactbase.Member> members, List<int> awayMembers, List<Data.Models.EventGuest> eventMemberIds)
        {
            var memberSection = new MealSetupSectionModel();

            // Cook data for household
            memberSection.Households = members.GroupBy(x => x.HouseholderId).Select(x => new HousehosmartFundsodel
            {
                HouseholderId = x.First().HouseholderId.GetValueOrDefault(),
                HouseholdName = x.First().HouseholdName,
                Members = x.Select(y => new MemberInformationModel
                {
                    EventGuestId = eventMemberIds.Where(z => z.MemberId == y.Id).Select(z => z.Id).FirstOrDefault(),
                    MemberId = y.Id,
                    Title = y.Title,
                    FullName = y.FullName,
                    Age = y.Age.GetValueOrDefault(),
                    IsAway = awayMembers.Contains(y.Id),
                    IsHouseholder = y.IsHouseholder,
                    ContactNumber = y.HomePhone,
                    EmailAddress = y.Email
                }).OrderByDescending(z => z.IsHouseholder).ThenByDescending(z => z.Age).ToList(),
                HousehosmartFundsembers = x.Count(y => !awayMembers.Contains(y.Id)),
                Region = new Region
                {
                    Id = x.First().RegionId.GetValueOrDefault(),
                    Name = x.First().RegionName
                },
                Locality = new Locality
                {
                    Id = x.First().LocalityId.GetValueOrDefault(),
                    Name = x.First().LocalityName
                },
                Sublocality = new Sublocality
                {
                    Id = x.First().SublocalityId ?? 0,
                    Name = x.First().SublocalityName
                }
            }).OrderBy(x => x.HouseholdName).ToList();

            // Cook data for locality
            var localities = members.Select(x => x.Locality).GroupBy(x => x.Id).Select(x => x.First()).OrderBy(x => x.Name).ToList();
            var sublocaloties = members.Select(x => x.Sublocality).GroupBy(x => x.Id).Select(x => x.First()).ToList();
            foreach (var locality in localities)
            {
                locality.Sublocalities = sublocaloties.Where(x => x.LocalityId == locality.Id).OrderBy(x => x.Name).ToList();
            }
            memberSection.Localities = _mapper.Map<List<Locality>>(localities);

            return memberSection;
        }

        /// <summary>
        /// Populate data for tba section in Meal Set-up screen
        /// </summary>
        /// <param name="members"></param>
        /// <param name="awayMembers"></param>
        /// <param name="eventMemberIds"></param>
        /// <param name="guestHouseholdIds"></param>
        /// <param name="householdOfEvents"></param>
        /// <param name="eventDate"></param>
        /// <returns></returns>
        private MealSetupSectionModel PopulateEventTba(List<Data.Models.Contactbase.Member> members, List<int> awayMembers,
            List<Data.Models.EventGuest> eventMemberIds, List<int> guestHouseholdIds, List<HouseholdConsecMeal> householdOfEvents, DateTime eventDate)
        {
            var memberSection = new MealSetupSectionModel();

            // Cook data for household
            memberSection.Households = members.GroupBy(x => x.HouseholderId).Select(x => new HousehosmartFundsodel
            {
                HouseholderId = x.First().HouseholderId.GetValueOrDefault(),
                HouseholdName = x.First().HouseholdName,
                Members = x.Select(y => new MemberInformationModel
                {
                    EventGuestId = eventMemberIds.Where(z => z.MemberId == y.Id).Select(z => z.Id).FirstOrDefault(),
                    MemberId = y.Id,
                    Title = y.Title,
                    FullName = y.FullName,
                    Age = y.Age.GetValueOrDefault(),
                    IsAway = awayMembers.Contains(y.Id),
                    IsHouseholder = y.IsHouseholder,
                    WarningToChange = guestHouseholdIds.Contains(y.HouseholderId ?? 0)
                }).OrderByDescending(z => z.IsHouseholder).ThenByDescending(z => z.Age).ToList(),
                HousehosmartFundsembers = x.Count(y => !awayMembers.Contains(y.Id)),
                Locality = new Locality
                {
                    Id = x.First().LocalityId.GetValueOrDefault(),
                    Name = x.First().LocalityName
                },
                Sublocality = new Sublocality
                {
                    Id = x.First().SublocalityId ?? 0,
                    Name = x.First().SublocalityName
                }

            }).OrderBy(x => x.HouseholdName).ToList();

            // Populate consec meal
            foreach (var household in memberSection.Households)
            {
                var eventOfHousehold = householdOfEvents.Where(x => x.HouseholderId == household.HouseholderId).OrderByDescending(x => x.EventDate).ToList();
                
               // household.ConsecMeal = Infrastructure.CountConsecMeal(eventDate, eventOfHousehold.Select(x => x.EventDate).ToList());
            }

            // Cook data for locality
            var localities = members.Select(x => x.Locality).GroupBy(x => x.Id).Select(x => x.First()).OrderBy(x => x.Name).ToList();
            var sublocaloties = members.Select(x => x.Sublocality).GroupBy(x => x.Id).Select(x => x.First()).ToList();
            foreach (var locality in localities)
            {
                locality.Sublocalities = sublocaloties.Where(x => x.LocalityId == locality.Id).OrderBy(x => x.Name).ToList();
            }
            memberSection.Localities = _mapper.Map<List<Locality>>(localities);

            return memberSection;
        }

        /// <summary>
        /// Add host and event host if they aren't exist
        /// </summary>
        /// <param name="householderIds"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        private async Task AddHostAndEventHost(List<int> householderIds, int eventId)
        {
            var existHouseholdIds = (await _unitOfWork.HostRepository.FindByAsync(x => householderIds.Contains(x.HouseholderId))).Select(x => x.HouseholderId);

            var newHouseholdIds = householderIds.Except(existHouseholdIds).ToList();
            if (newHouseholdIds.Any())
            {
                _unitOfWork.HostRepository.BulkInsert(newHouseholdIds.Select(x => new Host
                {
                    HouseholderId = x,
                    DefaultCP = 0,
                    DefaultSCP = 0
                }).ToList());

                await _unitOfWork.SaveChangesAsync();
            }

            var hosts = await _unitOfWork.HostRepository.FindByAsync(x => householderIds.Contains(x.HouseholderId));
            var allMemberInHouseholds = await _unitOfWork.MemberRepository.FindByAsync(x => householderIds.Contains(x.HouseholderId.Value) && !x.IsDeceased && !x.IsHidden);
            var eventHosts = new List<Data.Models.EventHost>();
            foreach (var householderId in householderIds)
            {
                var host = hosts.FirstOrDefault(y => y.HouseholderId == householderId);
                if (host == null) continue;

                var isAllMemberOverAllowedAges = allMemberInHouseholds.Where(x => x.HouseholderId == householderId).All(x => x.Age >= 80);
                if (!isAllMemberOverAllowedAges)
                {
                    eventHosts.Add(new Data.Models.EventHost
                    {
                        HostId = host.Id,
                        HouseholderId = householderId,
                        EventId = eventId,
                        CP = host.DefaultCP,
                        SCP = host.DefaultSCP
                    });
                }
                    
            }
            _unitOfWork.EventHostRepository.BulkInsert(eventHosts);
            await _unitOfWork.SaveChangesAsync();
        }

        #endregion
    }
}

