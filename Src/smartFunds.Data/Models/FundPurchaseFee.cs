using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace smartFunds.Data.Models
{
    public class FundPurchaseFee
    {
        public int Id { get; set; }
        public int FundId { get; set; }
        [ForeignKey("FundId")]
        public Fund Fund { get; set; }
        public FromLabel FromLabel { get; set; }
        public decimal From { get; set; }
        public ToLabel ToLabel { get; set; }
        public decimal To { get; set; }
        public decimal Fee { get; set; }
    }
}
