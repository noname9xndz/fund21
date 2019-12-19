using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using smartFunds.Common.Data.Repositories;
using smartFunds.Data.Models.Contactbase;

namespace smartFunds.Data.Models
{
    public class Interchange : IPersistentEntity, ITrackedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public int MainLocalityId { get; set; }
        public string EmailAddress { get; set; }
        public bool IsDeleted { get; set; }
        [MaxLength(30)]
        public string DeletedAt { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        [ForeignKey("MainLocalityId")]
        public Locality Locality { get; set; }
        [ForeignKey("CountryCode")]
        public Country Country { get; set; }
        public ICollection<InterchangeLocality> InterchangeLocalities { get; set; }
    }  
}
