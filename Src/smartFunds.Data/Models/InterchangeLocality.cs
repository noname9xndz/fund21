using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using smartFunds.Common.Data.Repositories;
using smartFunds.Data.Models.Contactbase;

namespace smartFunds.Data.Models
{
    public class InterchangeLocality: IPersistentEntity, ITrackedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int InterchangeId { get; set; }
        public int LocalityId { get; set; }
        public bool IsDeleted { get; set; }
        [MaxLength(30)]
        public string DeletedAt { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        [ForeignKey("InterchangeId")]
        public Interchange Interchange { get; set; }
        [ForeignKey("LocalityId")]
        public Locality Locality { get; set; }
    }
}