using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace smartFunds.Data.Models
{
    public class InvestmentTargetSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PortfolioId { get; set; }
        [ForeignKey("PortfolioId")]
        public Portfolio Portfolio  { get; set; }
        public Duration Duration { get; set; }
        public decimal Value { get; set; }

    }
}
