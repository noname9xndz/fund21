using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace smartFunds.Presentation.Models
{
    public class PassMealSearchModel
    {
        [Required]
        public int EventHostId { get; set; }
        [Required]
        public List<int> MemberIds { get; set; }
    }
}
