using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace smartFunds.Presentation.Models
{
    public class SublocalitySearchModel
    {
        [Required]
        public List<int> LocalityIds { get; set; }
    }
}
