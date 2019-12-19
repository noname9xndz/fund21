using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Model.Common
{
    public class FundPurchaseFeeModel
    {
        public int Id { get; set; }
        public int FundId { get; set; }
        public FromLabel FromLabel { get; set; }
        public decimal From { get; set; }
        public ToLabel ToLabel { get; set; }
        public decimal To { get; set; }
        public decimal Fee { get; set; }
    }
}
