using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace smartFunds.Presentation.Models
{
    public class EventMemberRoleModel
    {
        [Required]
        public List<int> EventGuestIds { get; set; }
        [Required]
        public List<int> HouseholderIds { get; set; }
        public bool IsHost { get; set; }
        public bool IsTba { get; set; }
        public bool IsAway { get; set; }
        [Required]
        public int EventId { get; set; }
    }
}
