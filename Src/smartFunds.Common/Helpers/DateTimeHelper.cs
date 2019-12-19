using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace smartFunds.Common.Helpers
{
    public static class DateTimeHelper
    {
        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            //int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            TimeSpan time = startDate - endDate;
            return Math.Abs(time.Days/30);
        }
    }


}
