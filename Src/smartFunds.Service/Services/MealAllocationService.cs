using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using smartFunds.Common.Exceptions;
using smartFunds.Data.Models;
using smartFunds.Data.UnitOfWork;
using smartFunds.Service.Models;

namespace smartFunds.Service.Services
{
    public interface IMealAllocationService
    {
        Task<List<MealHostAllocation>> GetMealHostAllocation(int eventId);
        Task CancelAllocation(List<int> eventHostIds);
        Task<MealAllocationStatistics> GetMealAllocationStatistics(int eventId);
        Task<List<HouseholdAllocated>> GetHouseholdAllocatedInHost(int eventHostId);
        Task<List<DateTime>> GetPassMealWithCurrentHost(PassMealSearch model);
    }

    public class MealAllocationService : IMealAllocationService
    {
        private readonly IUnitOfWork _unitOfWork;

        #region Contructor

        public MealAllocationService(IUnitOfWork unitOfWork = null)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Method

        /// <summary>
        /// Get meal host allocation information of event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<List<MealHostAllocation>> GetMealHostAllocation(int eventId)
        {
            var @event = await _unitOfWork.EventRepository.GetAsync(x => x.Id == eventId);
            if (@event == null) throw new MissingParameterException();

            var mealHosts = await _unitOfWork.MealAllocationRepository.GetMealHostAllocation(eventId);

            return mealHosts;
        }

        /// <summary>
        /// Cancel allocation
        /// </summary>
        /// <param name="eventHostIds"></param>
        /// <returns></returns>
        public async Task CancelAllocation(List<int> eventHostIds)
        {
            var mealAllocations = await _unitOfWork.MealAllocationRepository.FindByAsync(x => eventHostIds.Contains(x.EventHostId));
            if (mealAllocations == null || !mealAllocations.Any()) throw new MissingParameterException();

            _unitOfWork.MealAllocationRepository.BulkDelete(mealAllocations);
            await _unitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// Get acceptance criteria of event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<MealAllocationStatistics> GetMealAllocationStatistics(int eventId)
        {
            var @event = await _unitOfWork.EventRepository.GetAsync(x => x.Id == eventId);
            if (@event == null) return null;

            // Get member in event and seperate by type
            var eventMembers = (await _unitOfWork.EventGuestRepository.FindByAsync(x => x.EventId == eventId)).ToList();
            var eventHosts = (await _unitOfWork.EventHostRepository.FindByAsync(x => x.EventId == eventId)).ToList();
            var eventHostHouseholdIds = eventHosts.Select(x => x.HouseholderId).Distinct().ToList();

            var eventHostIds = eventHosts.Select(x => x.Id).ToList();
            var guestAllocated = (await _unitOfWork.MealAllocationRepository.FindByAsync(x => eventHostIds.Contains(x.EventHostId))).ToList();

            var householdCapacity = await _unitOfWork.EventHostRepository.GetHouseholdCapacity(eventHostHouseholdIds, @event.Id);
            var hostHouseholdCapacity = householdCapacity.Where(x => eventHostHouseholdIds.Contains(x.HouseholderId)).ToList();

            var mealAllocationStatistics = new MealAllocationStatistics
            {
                TotalMealRequired = eventMembers.Count(x => !x.IsToBeAssigned && !x.IsAway),
                TotalMinAvailable = hostHouseholdCapacity.Sum(x => x.CP),
                TotalMaxAvailable = hostHouseholdCapacity.Sum(x => x.SCP),
                GuestAllocated = guestAllocated.Count
            };

            return mealAllocationStatistics;
        }

        /// <summary>
        /// Get all household allocated into host
        /// </summary>
        /// <param name="eventHostId"></param>
        /// <returns></returns>
        public async Task<List<HouseholdAllocated>> GetHouseholdAllocatedInHost(int eventHostId)
        {
            var guestAllocated = await _unitOfWork.MealAllocationRepository.GetHouseholdAllocatedInHost(eventHostId);

            return guestAllocated;
        }

        /// <summary>
        /// Get all pass meal with current host for members
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<DateTime>> GetPassMealWithCurrentHost(PassMealSearch model)
        {
            // Get host householder id
            var hostHouseholder = await _unitOfWork.EventHostRepository.GetAsync(x => x.Id == model.EventHostId);
            if (hostHouseholder == null) throw new MissingParameterException();
            var hostHouseholderId = hostHouseholder.Id;
            var currentEventId = hostHouseholder.EventId;

            // Find event which attent meal together
            var oldEventHostIds = (await _unitOfWork.EventHostRepository.FindByAsync(x => x.HouseholderId == hostHouseholderId && x.EventId != currentEventId)).Select(x => x.Id);
            var oldEventGuests = await _unitOfWork.EventGuestRepository.FindByAsync(x => model.MemberIds.Contains(x.MemberId) && x.EventId != currentEventId);
            var oldEventGuestIds = oldEventGuests.Select(x => x.Id);
            var passMealTogethers = (await _unitOfWork.MealAllocationRepository.FindByAsync(x => oldEventHostIds.Contains(x.EventHostId) && oldEventGuestIds.Contains(x.EventGuestId))).Select(x => x.EventGuestId);
            var passEventTogethers = oldEventGuests.Where(x => passMealTogethers.Contains(x.Id)).Select(x => x.EventId).Distinct().ToList();

            // Find pass meal date
            var passMealDates = (await _unitOfWork.EventRepository.FindByAsync(x => passEventTogethers.Contains(x.Id))).Select(x => x.EventDate).OrderBy(x => x);

            return passMealDates.ToList();
        }

        #endregion

    }
}
