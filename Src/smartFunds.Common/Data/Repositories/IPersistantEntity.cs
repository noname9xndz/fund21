using System;

namespace smartFunds.Common.Data.Repositories
{
    public interface IPersistentEntity
    {
        bool IsDeleted { get; set; }
        string DeletedAt { get; set; } 
    }
}
