using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Model.Common
{
    public class StatisticModel
    {
        public decimal TotalAccountFee { get; set; }

        public decimal TotalWithdrawalFee { get; set; }

        public decimal TotalQuickWithdrawal { get; set; }

        public string TransactionDateFrom { get; set; }

        public string TransactionDateTo { get; set; }
    }
}
