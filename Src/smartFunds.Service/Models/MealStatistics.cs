using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Service.Models
{
    public class MealStatistics
    {
        public int TotalHouseholds { get; set; }
        public int NoInHostHouseholds { get; set; }
        public int TotalCP { get; set; }
        public int TotalSCP { get; set; }
        public int TotalGuests { get; set; }
        public int TotalPersonsTba { get; set; }
        public double AverageBreakSize { get; set; }
        public int Discrepancy { get; set; }
    }
}
