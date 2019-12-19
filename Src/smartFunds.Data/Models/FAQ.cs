using smartFunds.Common.Data.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartFunds.Data.Models
{
    public class FAQ : ITrackedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }        
        public string Content { get; set; }       
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public ICollection<FAQ> RelatedFAQs { get; set; }
    }
}
