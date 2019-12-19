using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Data.Models
{
    public class HouseholdCapacity
    {
        public int HouseholderId { get; set; }
        public int CP { get; set; }
        public int SCP { get; set; }
        public int DefaultCP { get; set; }
        public int DefaultSCP { get; set; }
    }
}
