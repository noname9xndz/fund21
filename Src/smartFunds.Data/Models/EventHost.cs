using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using smartFunds.Common.Data.Repositories;

namespace smartFunds.Data.Models
{
    public class EventHost : ITrackedEntity, IPersistentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EventId { get; set; }
        public int HostId { get; set; }
        public int CP { get; set; }
        public int SCP { get; set; }
        public int HouseholderId { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }
        [ForeignKey("HostId")]
        public Host Host { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        [MaxLength(30)]
        public string DeletedAt { get; set; }
    }
}
