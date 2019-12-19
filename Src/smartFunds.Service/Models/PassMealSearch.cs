using System.Collections.Generic;

namespace smartFunds.Service.Models
{
    public class PassMealSearch
    {
        public int EventHostId { get; set; }
        public List<int> MemberIds { get; set; }
    }
}
