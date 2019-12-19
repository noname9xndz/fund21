using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace smartFunds.Presentation.Models
{
    public class GuestSearchModel
    {
        [Required]
        public string SearchPhase { get; set; }
        public List<string> CountryCodes { get; set; }
        public List<int> LocalityIds { get; set; }
        public List<int> SublocalityIds { get; set; }
        [Required]
        public int EventId { get; set; }
    }
}
