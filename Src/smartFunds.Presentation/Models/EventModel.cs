using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace smartFunds.Presentation.Controllers
{
    public class EventInputModel
    {        
        [Required]
        public string CountryCode { get; set; }
        [Required]
        public DateTime EventDate { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Invalid main locality Id")]
        public int MainLocalityId { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "At least 1 sub locality must be selected")]
        public List<int> SublocalityIds { get; set; }
    }
}