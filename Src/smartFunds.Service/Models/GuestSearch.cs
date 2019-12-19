using System.Collections.Generic;

namespace smartFunds.Service.Models
{
    public class GuestSearch
    {
        public string SearchPhase { get; set; }
        public List<string> CountryCodes { get; set; }
        public List<int> LocalityIds { get; set; }
        public List<int> SublocalityIds { get; set; }
        public int EventId { get; set; }
    }

    public class MemberResult
    {
        public int MemberId { get; set; }
        public string Title { get; set; }
        public string FirtName { get; set; }
        public string LastName { get; set; }
        public int HouseholderId { get; set; }
        public string HouseholdName { get; set; }
        public int Age { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public int LocalityId { get; set; }
        public string LocalityName { get; set; }
        public string Role { get; set; }
        public string PhotoPath { get; set; }
        public string CountryCode { get; set; }
    }
}
