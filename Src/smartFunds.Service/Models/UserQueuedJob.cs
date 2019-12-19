using smartFunds.Service.Models.HangFire;
using System;

namespace smartFunds.Service.Models
{
    public class UserQueuedJob
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string JobId { get; set; }
        public Job Job { get; set; }
    }
}
