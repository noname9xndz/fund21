using Microsoft.EntityFrameworkCore;

namespace smartFunds.Data.UnitOfWork
{
    public class ChangeTrackingEntity
    {
        public EntityState State { get; set; }
        public object Entity { get; set; }
        public object OriginalValue { get; set; }
    }
}
