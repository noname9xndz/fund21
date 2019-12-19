using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace smartFunds.Presentation.Controllers
{
    public class InterchangeInputModel
    {        
        [Required]
        public string CountryCode { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Invalid main locality Id")]
        public int MainLocalityId { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string EmailAddress { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "At least 1 locality must be selected")]
        public List<int> LocalityIds { get; set; }
    }
}