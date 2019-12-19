using smartFunds.Common;
using System;

namespace smartFunds.Data.Models
{
    public class PortfolioFund
    {
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }
        public int FundId { get; set; }
        public Fund Fund { get; set; }
        public decimal? FundPercent { get; set; }
        public decimal? FundPercentNew { get; set; }
        public EditStatus EditStatus { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
