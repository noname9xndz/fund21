using System.Collections.Generic;
using System.Linq;
using smartFunds.Common;

namespace smartFunds.Service.Models
{
    public class User
    {
        public string UserId { get; set; }      
        public string UserName { get; set; }            
        public int MemberId { get; set; }
        public string EmailAddress { get; set; }
        public int LocalityId { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public Member Member { get; set; }

        public bool IsInRole(RoleType role)
        {
            return Roles.Any(x => x == role.ToString());
        }
    }
}