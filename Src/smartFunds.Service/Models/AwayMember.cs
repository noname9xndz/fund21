using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Service.Models
{
    public class AwayMember
    {
        public int EventGuestId { get; set; }
        public int MemberId { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public int HouseholderId { get; set; }
        public string Locality { get; set; }
        public string CountryCode { get; set; }
        public int LocalityId { get; set; }
        public int SublocalityId { get; set; }
        public string SublocalityName { get; set; }
    }

    public class AwayList
    {
        public List<AwayMember> AwayMembers { get; set; }
        public List<Locality> Localities { get; set; }
    }
}
