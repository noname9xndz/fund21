using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Models
{
    public class InvestmentViewModel
    {
        public decimal? Amount { get; set; }

        public string SenderMsisdn { get; set; }

        public bool IsInvestTarget { get; set; }

        public bool Success { get; set; }
    }
}
