using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace smartFunds.Presentation.Models
{
    public class MemberHostModel
    {
        [Required]
        public List<int> HouseholderIds { get; set; }
        [Required]
        public int EventId { get; set; }
    }
}
