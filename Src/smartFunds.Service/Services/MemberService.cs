using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using smartFunds.Common;
using smartFunds.Data.UnitOfWork;
using smartFunds.Service.Models;
using Member = smartFunds.Data.Models.Contactbase.Member;

namespace smartFunds.Service.Services
{
    public interface IMemberService
    {
        Task<WebSearchResult<MemberResult>> SearchMemberForAssignEventGuest(GuestSearch search);
        Task<WebSearchResult<HostResult>> SearchHostForAssignEventHost(HostSearch search);

    }
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Contructor

        public MemberService(IUnitOfWork unitOfWork = null)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Search member to be assign as guest in event
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<WebSearchResult<MemberResult>> SearchMemberForAssignEventGuest(GuestSearch search)
        {
            var @event = await _unitOfWork.EventRepository.GetAsync(x => x.Id == search.EventId);
            if (@event == null) return null;

            search.SearchPhase = search.SearchPhase.ToLower();
            var listKeyWord = search.SearchPhase.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var members = await _unitOfWork.MemberRepository.FindByAsync(x => x.IsHidden == false && x.IsDeceased == false &&
                                                                      (search.CountryCodes == null || !search.CountryCodes.Any() || search.CountryCodes.Contains(x.CountryCode)) &&
                                                                      (search.LocalityIds == null || !search.LocalityIds.Any() || search.LocalityIds.Contains(x.LocalityId ?? 0)) &&
                                                                      (search.SublocalityIds == null || !search.SublocalityIds.Any() || search.SublocalityIds.Contains(x.SublocalityId ?? 0)) &&
                                                                      (listKeyWord.All(s => (x.Title + " " + x.FullName).ToLower().Contains(s)) ||
                                                                       listKeyWord.All(s => (x.Title + " " + x.FullNameReverse).ToLower().Contains(s)) ||
                                                                       x.HouseholdCoupleName.ToLower().Contains(search.SearchPhase) ||
                                                                       x.HouseholdName.ToLower().Contains(search.SearchPhase)
                                                                       ));

            if (members == null || !members.Any()) return null;
            // Order
            members = members.OrderBy(x => x.HouseholderId).ThenByDescending(x => x.IsHouseholder).ThenByDescending(x => x.Age).ToList();

            var householderIds = members.Select(x => x.HouseholderId ?? 0).Distinct().ToList();
            var eventInSameDateIds = (await _unitOfWork.EventRepository.FindByAsync(x => x.EventDate == @event.EventDate && x.Id != @event.Id)).Select(x => x.Id);
            var eventMembers = await _unitOfWork.EventGuestRepository.FindByAsync(x => (eventInSameDateIds.Contains(x.EventId) || x.EventId == @event.Id) && householderIds.Contains(x.HouseholderId));
            var eventHosts = await _unitOfWork.EventHostRepository.FindByAsync(x => (eventInSameDateIds.Contains(x.EventId) || x.EventId == @event.Id) && householderIds.Contains(x.HouseholderId));
            
            var eventGuestIds = eventMembers.Where(x => !x.IsAway && !x.IsToBeAssigned && x.EventId == @event.Id).Select(x => x.MemberId).ToList();
            var eventAwayIds = eventMembers.Where(x => x.IsAway && x.EventId == @event.Id).Select(x => x.MemberId).ToList();
            var eventTbaIds = eventMembers.Where(x => x.IsToBeAssigned && x.EventId == @event.Id).Select(x => x.MemberId).ToList();
            var eventHostIds = eventHosts.Where(x => x.EventId == @event.Id).Select(x => x.HouseholderId).ToList();


            var otherEventIds = eventMembers.Where(x => eventInSameDateIds.Contains(x.EventId)).Select(x => x.HouseholderId).Distinct().ToList();
            var otherHostIds = eventHosts.Where(x => eventInSameDateIds.Contains(x.EventId)).Select(x => x.HouseholderId).ToList();
            otherHostIds.AddRange(otherEventIds);

            var result = new WebSearchResult<MemberResult>
            {
                TotalCount = members.Count,
                Result = members.Select(x => new MemberResult
                {
                    MemberId = x.Id,
                    Title = x.Title,
                    FirtName = x.FirstName,
                    LastName = x.LastName,
                    HouseholderId = x.HouseholderId ?? 0,
                    HouseholdName = x.HouseholdName,
                    Age = x.Age ?? 0,
                    RegionId = x.RegionId ?? 0,
                    RegionName = x.RegionName,
                    LocalityId = x.LocalityId ?? 0,
                    LocalityName = x.LocalityName,
                    Role = GetMemberRoleInEvent(x, eventHostIds, eventGuestIds, eventAwayIds, eventTbaIds, otherHostIds),
                    PhotoPath = x.PhotoTagCdnPath,
                    CountryCode = x.CountryCode
                })
            };

            return result;
        }

        /// <summary>
        /// Search host to assign as host in event meal
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<WebSearchResult<HostResult>> SearchHostForAssignEventHost(HostSearch search)
        {
            var @event = await _unitOfWork.EventRepository.GetAsync(x => x.Id == search.EventId);
            if (@event == null) return null;

            var hosts = await _unitOfWork.MemberRepository.FindByAsync(x => x.IsHidden == false && x.IsDeceased == false && x.Age.Value < 80 &&//x.IsHouseholder &&
                                                                              (search.LocalityId == 0 || x.LocalityId == search.LocalityId) &&
                                                                              (search.SublocalityIds == null || !search.SublocalityIds.Any() || search.SublocalityIds.Contains(x.SublocalityId ?? 0)) &&
                                                                              (x.HouseholdName.ToLower().Contains(search.SearchPhase) ||
                                                                               x.HouseholdNameDisplay.ToLower().Contains(search.SearchPhase) ||
                                                                               x.HouseholderName.ToLower().Contains(search.SearchPhase)));

            if (hosts == null || !hosts.Any()) return null;
            // Order
            hosts = hosts.GroupBy(x => x.HouseholderId).Select(x => x.First()).OrderBy(x => x.HouseholdName).ToList();

            // Find member in household if they already join as guest or host
            var householderId = hosts.Select(x => x.HouseholderId).Distinct().ToList();
            

            var eventInSameDateIds = (await _unitOfWork.EventRepository.FindByAsync(x => x.EventDate == @event.EventDate && x.Id != @event.Id)).Select(x => x.Id);
            var eventMembers = await _unitOfWork.EventGuestRepository.FindByAsync(x => (eventInSameDateIds.Contains(x.EventId) || x.EventId == @event.Id) && householderId.Contains(x.HouseholderId));
            var eventHosts = await _unitOfWork.EventHostRepository.FindByAsync(x => (eventInSameDateIds.Contains(x.EventId) || x.EventId == @event.Id) && householderId.Contains(x.HouseholderId));

            var guestHouseholderIds = eventMembers.Where(x => !x.IsAway && !x.IsToBeAssigned && x.EventId == @event.Id).Select(x => x.HouseholderId).Distinct().ToList();
            var tbaHouseholderIds = eventMembers.Where(x => x.IsToBeAssigned && x.EventId == @event.Id).Select(x => x.HouseholderId).Distinct().ToList();
            var hostHouseholderIds = eventHosts.Where(x => x.EventId == @event.Id).Select(x => x.HouseholderId).Distinct().ToList();

            var otherEventHouseholdIds = eventMembers.Where(x => eventInSameDateIds.Contains(x.EventId)).Select(x => x.HouseholderId).Distinct().ToList();
            var otherEventHostHouseholdIds = eventHosts.Where(x => eventInSameDateIds.Contains(x.EventId)).Select(x => x.HouseholderId).Distinct().ToList();
            otherEventHouseholdIds.AddRange(otherEventHostHouseholdIds);

            var hostResult = new List<HostResult>();
            foreach (var host in hosts)
            {
                var role = GetHostRoleInEvent(host.HouseholderId ?? 0, hostHouseholderIds, guestHouseholderIds, tbaHouseholderIds, otherEventHouseholdIds);
                hostResult.Add(new HostResult
                {
                    HouseholderId = host.HouseholderId ?? 0,
                    HouseholdName = host.HouseholdName,
                    Locality = $"{host.LocalityName} - {host.CountryCode}",
                    Role = role,
                    IsWarning = role.Equals(Constants.EventMemberRole.AlreadyGuest) || role.Equals(Constants.EventMemberRole.AlreadyTba),
                    UnableToAdd = role.Equals(Constants.EventMemberRole.AlreadyHost) || role.Equals(Constants.EventMemberRole.AlreadyInOtherEvent)
                });
            }

            return new WebSearchResult<HostResult>
            {
                TotalCount = hosts.Count,
                Result = hostResult
            };
        }

        #endregion

        #region Private Method

        /// <summary>
        /// Get role of member in event
        /// </summary>
        /// <param name="member"></param>
        /// <param name="hostIds"></param>
        /// <param name="guestIds"></param>
        /// <param name="awayIds"></param>
        /// <param name="tbaIds"></param>
        /// <param name="otherEventIds"></param>
        /// <returns></returns>
        private string GetMemberRoleInEvent(Member member, List<int> hostIds, List<int> guestIds, List<int> awayIds, List<int> tbaIds, List<int> otherEventIds)
        {
            if (hostIds.Contains(member.HouseholderId ?? 0))
                return Constants.EventMemberRole.AlreadyHost;
            if (guestIds.Contains(member.Id))
                return Constants.EventMemberRole.AlreadyGuest;
            if (awayIds.Contains(member.Id))
                return Constants.EventMemberRole.Away;
            if (tbaIds.Contains(member.Id))
                return Constants.EventMemberRole.PersonTba;
            if (otherEventIds.Contains(member.Id))
                return Constants.EventMemberRole.AlreadyInOtherEvent;
            return string.Empty;
        }

        /// <summary>
        /// Get role of member in event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hostIds"></param>
        /// <param name="guestIds"></param>
        /// <param name="tbaIds"></param>
        /// <param name="otherEventIds"></param>
        /// <returns></returns>
        private string GetHostRoleInEvent(int id, List<int> hostIds, List<int> guestIds, List<int> tbaIds, List<int> otherEventIds)
        {
            if (hostIds.Contains(id))
                return Constants.EventMemberRole.AlreadyHost;
            if (guestIds.Contains(id))
                return Constants.EventMemberRole.AlreadyGuest;
            if (tbaIds.Contains(id))
                return Constants.EventMemberRole.AlreadyTba;
            if (otherEventIds.Contains(id))
                return Constants.EventMemberRole.AlreadyInOtherEvent;
            return string.Empty;
        }

        #endregion
    }
}
