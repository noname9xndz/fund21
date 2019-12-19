using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Service.Models
{
    public class MealAllocationStatistics
    {
        public int TotalMinAvailable { get; set; } // Total of all Host CP
        public int TotalMaxAvailable { get; set; } // Total of all Host SCP
        public int TotalMealRequired { get; set; } // Total of all Guest Household members attending that event
        public int GuestAllocated { get; set; }

        public int MinAvailableSpace => TotalMinAvailable - GuestAllocated;
        public int MaxAvailableSpace => TotalMaxAvailable - GuestAllocated;

    }
}
