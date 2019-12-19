using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace smartFunds.Data.Models
{
    public class WithdrawalFee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TimeInvestmentBegin { get; set; }
        public int TimeInvestmentEnd { get; set; }
        [Column(TypeName = "decimal(12,4)")]
        public decimal Percentage { get; set; }
    }
}
