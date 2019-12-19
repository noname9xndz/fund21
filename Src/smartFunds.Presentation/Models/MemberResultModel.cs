using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Models
{
    public class MemberResultModel
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
        public string MemberPhotoUrl { get; set; }
        public string CountryCode { get; set; }
    }
}
