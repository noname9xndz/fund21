
using System.Collections.Generic;

namespace smartFunds.Data.Models
{
    public class MealHostAllocation
    {
        public int MealAllocationId { get; set; }
        public int EventHostId { get; set; }
        public string HouseholdName { get; set; }
        public int CP { get; set; }
        public int SCP { get; set; }
        public int GuestAllocated { get; set; }
        public int MinimumAvailableSpace => CP - GuestAllocated;
        public int MaximumAvailableSpace => SCP - GuestAllocated;
    }

    public class HouseholdAllocated
    {
        public int EventHostId { get; set; }
        public string HouseholdName { get; set; }
        public int TotalMember { get; set; }
        public IEnumerable<MemberAllocated> HousehosmartFundsembers { get; set; }
    }

    public class MemberAllocated
    {
        public int MemberId { get; set; }
        public string Title { get; set; }
        public string FistName { get; set; }
        public int Age { get; set; }
    }

}
