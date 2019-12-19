using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using smartFunds.Common.Data.Repositories;

namespace smartFunds.Data.Models
{
    public class MealAllocation : ITrackedEntity, IPersistentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EventHostId { get; set; }
        public int EventGuestId { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedAt { get; set; }
        [ForeignKey("EventHostId")]
        public EventHost EventHost { get; set; }
        [ForeignKey("EventGuestId")]
        public EventGuest EventGuest { get; set; }
    }
}
