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
    public interface IMealAllocationRepository : IRepository<MealAllocation>
    {
        Task<List<MealHostAllocation>> GetMealHostAllocation(int eventId);
        Task<List<HouseholdAllocated>> GetHouseholdAllocatedInHost(int eventHostId);
    }
    public class MealAllocationRepository : GenericPersistentTrackedRepository<MealAllocation>, IMealAllocationRepository
    {
        private readonly smartFundsDbContext _smartFundsDbContext;
        public MealAllocationRepository(smartFundsDbContext smartFundsDbContext, IOptions<smartFundsRedisOptions> redisConfigurationOptions
            , IRedisAutoCompleteProvider redisAutoCompleteProvider, IHttpContextAccessor httpContextAccessor) : base(smartFundsDbContext, redisConfigurationOptions, redisAutoCompleteProvider, httpContextAccessor)
        {
            _smartFundsDbContext = smartFundsDbContext;
        }

        /// <summary>
        ///  Get meal host allocated information of event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<List<MealHostAllocation>> GetMealHostAllocation(int eventId)
        {
            var result = (from r in (from h in _smartFundsDbContext.EventHosts
                                     join m in _smartFundsDbContext.Members on h.HouseholderId equals m.HouseholderId
                                     join a in _smartFundsDbContext.MealAllocations on h.Id equals a.EventHostId into tempA
                                     from a in tempA.DefaultIfEmpty()
                                     where h.EventId == eventId && m.IsHouseholder
                                     select new
                                     {
                                         h,
                                         m.HouseholdName,
                                         a
                                     })
                          group r by r.h.Id
                         into tempR
                          select new MealHostAllocation
                          {
                              MealAllocationId = tempR.Select(x => x.a.Id).FirstOrDefault(),
                              EventHostId = tempR.Key,
                              HouseholdName = tempR.Select(x => x.HouseholdName).FirstOrDefault(),
                              CP = tempR.Select(x => x.h.CP).FirstOrDefault(),
                              SCP = tempR.Select(x => x.h.SCP).FirstOrDefault(),
                              GuestAllocated = tempR.Select(x => x.a).FirstOrDefault() != null ? tempR.Count() : 0
                          }).OrderBy(x => x.HouseholdName);

            return await Task.FromResult(result.ToList());
        }

        /// <summary>
        /// Get all household located into host
        /// </summary>
        /// <param name="eventHostId"></param>
        /// <returns></returns>
        public async Task<List<HouseholdAllocated>> GetHouseholdAllocatedInHost(int eventHostId)
        {
            var result = (from r in (from a in _smartFundsDbContext.MealAllocations
                                     join g in _smartFundsDbContext.EventGuests on a.EventGuestId equals g.Id
                                     join m in _smartFundsDbContext.Members on g.MemberId equals m.Id
                                     where a.EventHostId == eventHostId
                                     select m)
                          group r by r.HouseholderId
                          into tempR
                          select new HouseholdAllocated
                          {
                              EventHostId = eventHostId,
                              HouseholdName = tempR.Select(x => x.HouseholdName).FirstOrDefault(),
                              TotalMember = tempR.Count(),
                              HousehosmartFundsembers = tempR.Select(x => new MemberAllocated
                              {
                                  MemberId = x.Id,
                                  Title = x.Title,
                                  FistName = x.FirstName,
                                  Age = x.Age ?? 0
                              })
                          }).OrderBy(x => x.HouseholdName);

            return await Task.FromResult(result.ToList());
        }
    }
}
