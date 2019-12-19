using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Model.Common
{
    public class FundSellFeeModel
    {
        public int Id { get; set; }
        public int FundId { get; set; }
        public FromLabel FromLabel { get; set; }
        public int From { get; set; }
        public ToLabel ToLabel { get; set; }
        public int To { get; set; }
        public decimal Fee { get; set; }
    }
}
