using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using smartFunds.Common.Data.Repositories;

namespace smartFunds.Data.Models
{
    public class EventGuest : IPersistentEntity, ITrackedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int EventId { get; set; }
        public int MemberId { get; set; }
        public int HouseholderId { get; set; }
        public bool IsAway { get; set; }
        public bool IsToBeAssigned { get; set; }
        public DateTime DateLastUpdated { get; set ; }
        public string LastUpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        [MaxLength(30)]
        public string DeletedAt { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }
    }
}
