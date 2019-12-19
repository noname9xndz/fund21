using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using smartFunds.Common.Data.Repositories;

namespace smartFunds.Data.Models
{
    public class KVRRQuestion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; }
        public int No { get; set; }
        public ICollection<KVRRAnswer> KVRRAnswers { get; set; }
    }
}
