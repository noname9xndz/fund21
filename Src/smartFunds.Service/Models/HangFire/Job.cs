using System;

namespace smartFunds.Service.Models.HangFire
{
    public class Job
    {
        public int Id { get; set; }
        public string StateName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpireAt { get; set; }
    }
}
