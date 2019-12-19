using System;

namespace smartFunds.Common.Data.Repositories
{
    public interface ITrackedEntity
    {
        DateTime DateLastUpdated { get; set; }
        string LastUpdatedBy { get; set; }
    }
}
