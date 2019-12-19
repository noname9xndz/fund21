using System;
using System.Collections.Generic;

namespace smartFunds.Common
{
    public static class Infrastructure
    {
        /// <summary>
        /// Count consec meal of household
        /// </summary>
        /// <param name="eventDate"></param>
        /// <param name="eventOfHousehold"></param>
        /// <returns></returns>
        public static int CountConsecMeal(DateTime eventDate, List<DateTime> eventOfHousehold)
        {
            var consec = 0;
            var currentDate = eventDate;

            foreach (var date in eventOfHousehold)
            {
                if ((int)(currentDate - date).TotalDays != 7) return consec;

                currentDate = date;
                consec++;
            }

            return consec;
        }
    }
}
