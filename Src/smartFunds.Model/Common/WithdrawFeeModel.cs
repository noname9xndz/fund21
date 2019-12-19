using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Model.Common
{
    public class WithdrawFeeModel
    {
        public int Id { get; set; }
        public int TimeInvestmentBegin { get; set; }
        public int TimeInvestmentEnd { get; set; }
        public decimal Percentage { get; set; }
    }
}
