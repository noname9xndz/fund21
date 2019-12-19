using smartFunds.Common;

namespace smartFunds.Caching
{
    public class BuildCacheInfo
    {
        public BuildCacheStatusType? Status { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Heartbeat { get; set; }
        public string Error { get; set; }
    }
}
