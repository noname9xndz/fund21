using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace smartFunds.Data.Models
{
    public class MaintainingFee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public decimal AmountFrom { get; set; }
        public decimal AmountTo { get; set; }
        [Column(TypeName = "decimal(12,4)")]
        public decimal Percentage { get; set; }
        [NotMapped]
        public FormState EntityState { get; set; }
    }
}
