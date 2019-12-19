using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using smartFunds.Common.Data.Repositories;
using smartFunds.Data.Models.Contactbase;

namespace smartFunds.Data.Models
{
    public class EventSublocality : IPersistentEntity, ITrackedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int EventId { get; set; }
        public int SublocalityId { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedAt { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }
        [ForeignKey("SublocalityId")]
        public Sublocality Sublocality { get; set; }

    }
}