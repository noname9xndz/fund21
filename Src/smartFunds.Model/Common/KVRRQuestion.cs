using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartFunds.Model.Common
{
    public class KVRRQuestion
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }        
        public IEnumerable<KVRRAnswer> Answers { get; set; }
        public int No { get; set; }
        [NotMapped]
        public int EntityState { get; set; }
    }
}
