using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Model.Common
{
    public class InvestmentFunds
    {
        public List<InvestmentFund> ListInvestmentFund { get; set; }

        public InvestmentFunds()
        {
            ListInvestmentFund = new List<InvestmentFund>();
        }
    }

    public class InvestmentFund
    {
        public string FundCode { get; set; }
        public string FundName { get; set; }
        public decimal TotalAmountUserFund { get; set; }
        public int CountUserFund { get; set; }
    }
}
