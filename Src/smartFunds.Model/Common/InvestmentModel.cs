using System;
using System.ComponentModel.DataAnnotations;

namespace smartFunds.Model.Common
{
    public class InvestmentModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public UserModel User { get; set; }

        public decimal Amount { get; set; }

        public decimal RemainAmount { get; set; }

        public DateTime DateInvestment { get; set; }
    }
}
