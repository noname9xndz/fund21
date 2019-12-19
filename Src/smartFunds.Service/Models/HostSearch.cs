using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Service.Models
{
    public class HostSearch
    {
        public string SearchPhase { get; set; }
        public int LocalityId { get; set; }
        public List<int> SublocalityIds { get; set; }
        public int EventId  { get; set; }
    }

    public class HostResult
    {
        public int HouseholderId { get; set; }
        public string HouseholdName { get; set; }
        public string Locality { get; set; }
        public string Role { get; set; }
        public bool UnableToAdd { get; set; }
        public bool IsWarning { get; set; }
    }
}
