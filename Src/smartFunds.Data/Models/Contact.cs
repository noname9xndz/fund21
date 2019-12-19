using smartFunds.Common.Data.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using smartFunds.Common;

namespace smartFunds.Data.Models
{
    public class Contact : ITrackedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FullName { get; set; }        
        public string Content { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
