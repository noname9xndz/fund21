using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using smartFunds.Common.Data.Repositories;

namespace smartFunds.Data.Models
{
    public class Host: ITrackedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int HouseholderId { get; set; }
        public int DefaultCP { get; set; }
        public int DefaultSCP { get; set; }
        public int LocalityId { get; set; }
        public ICollection<EventHost> EventHosts { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
