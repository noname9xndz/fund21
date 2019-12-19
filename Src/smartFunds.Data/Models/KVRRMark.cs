using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using smartFunds.Common;
using smartFunds.Common.Data.Repositories;

namespace smartFunds.Data.Models
{
    public class KVRRMark: ITrackedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int MarkFrom { get; set; }
        public int MarkTo { get; set; }
        public int? KVRRId { get; set; }
        [ForeignKey("KVRRId")]
        public KVRR KVRR { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        [NotMapped]
        public FormState EntityState { get; set; }
    }
}
