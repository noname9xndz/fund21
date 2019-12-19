using System;
using System.ComponentModel;

namespace smartFunds.Common
{
    public enum RoleType
    {
        [Description("Admin")]
        ADMIN = 1,
        [Description("smartFunds Admin")]
        smartFunds_ADMIN,
        [Description("smartFunds co_ordinator")]
        smartFunds_CO_ORDINATOR
    }

    public enum BuildCacheStatusType
    {
        [Description("INPROGRESS")]
        InProgress = 0,
        [Description("FINISHED")]
        Finished = 1,
        [Description("ERROR")]
        Error = 2,
        [Description("QUEUED")]
        Queued = 3,
    }

    public enum AutoCompleteType
    {
        Setting,
        Member,
        ContactBaseLocality,
        ContactBaseCountry
    }

    public static class EnumHelpers
    {
        public static T ToEnum<T>(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return default(T);
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
