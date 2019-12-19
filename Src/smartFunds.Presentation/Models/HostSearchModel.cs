using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace smartFunds.Presentation.Models
{
    public class HostSearchModel
    {
        [Required]
        public string SearchPhase { get; set; }
        public List<int> SublocalityIds { get; set; }
        [Required]
        public int EventId { get; set; }
        public int LocalityId { get; set; }
    }
}
