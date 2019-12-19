using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Model.Common
{
    public class InvestmentTargetSettingModel
    {
        public int Id { get; set; }
        public int PortfolioId { get; set; }
        public PortfolioModel Portfolio { get; set; }

        public Duration Duration { get; set; }
        public decimal Value { get; set; }
    }

}
