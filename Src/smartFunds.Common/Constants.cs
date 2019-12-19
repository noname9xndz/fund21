
namespace smartFunds.Common
{
    public static class Constants
    {
        public static class Database
        {
            public const string smartFundsConnectionStringName = "smartFundsDatabase";
            public const string ContactBaseConnectionStringName = "ContactBaseDatabase";
        }

        public static class CorsPolicy
        {
            public const string AllowAll = "AllowAll";
        }

        public static class RedisAutocomplete
        {
            public const string SetNameFormat = "AUTOC: {0}";
        }

        public static class Cache {
            public static class BuildKeys
            {
                public const string Status = "Build.Status";
                public const string Heartbeat = "Build.Heartbeat";
                public const string StartTime = "Build.StartTime";
                public const string EndTime = "Build.EndTime";
                public const string Error = "Build.Error";
            }

            public static class HeartbeatMessage
            {
                public const string StartsmartFunds = "Starting build smartFunds cache";
                public const string FinishsmartFunds = "Finished build smartFunds cache";
                public const string StartContactBase = "Starting build contactbase cache";
                public const string FinishContactBase = "Finished build smartFunds cache.EndTime";
            }
        }

        public static class DateTimeFormats
        {
            public const string IsoLongDateTime = "yyyy-MM-dd HH:mm:ss.ffffff";
        }

        public static class EventMemberRole
        {
            public const string AlreadyHost = "Already mark as host";
            public const string AlreadyGuest = "Already a guest";
            public const string Away = "Away";
            public const string PersonTba = "Person to be assigned";
            public const string AlreadyAway = "Already mark as away";
            public const string AlreadyTba = "Already mark as to be assigned";
            public const string AlreadyInOtherEvent = "Already Host / Guest on this date";
        }

        public static class Message
        {
            public const string UpdateTbaToHostError = "One or more Household member is assigned as Guest.";
            public const string UpdateGuestToHostWarnming = "Entire Household will be assigned as Host. Do you wish to Continue?";
            public const string SCPMustBiggerThanCP = "Host SCP must always bigger then Host CP";
        }

        public static class RoleName
        {
            public const string Admin = "admin";
            public const string Manager = "manager";
            public const string Customer = "customer";
        }
    }
}
