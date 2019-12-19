using System;
using System.Collections.Generic;
using System.Text;
using smartFunds.Data.Models;

namespace smartFunds.Service.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string EventName
        {
            get
            {
                return $"{EventDate.ToString("dd MMM yyyy")} {MainLocalityName} Lord's Day Meal";
            }
        }
        public int MainLocalityId { get; set; }
        public string MainLocalityName { get; set; }
        public DateTime EventDate { get; set; }
        public List<Sublocality> Sublocalities { get; set; }
        public Region Region { get; set; }
    }
}
