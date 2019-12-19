using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace smartFunds.Model.Common
{
    public class TransactionHistoryModel
    {
        public int Id { get; set; }

        public UserModel User { get; set; }

        public decimal CurrentAccountAmount { get; set; }

        [Display(Name = "Type", ResourceType = typeof(Resources.Common))]
        public TransactionType TransactionType { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resources.Common))]
        public decimal Amount { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Resources.Common))]
        public TransactionStatus Status { get; set; }

        [Display(Name = "TimeAction", ResourceType = typeof(Resources.Common))]
        public DateTime TransactionDate { get; set; }

        public decimal TotalWithdrawal { get; set; }

        public WithdrawalType? WithdrawalType { get; set; }

        public RemittanceStatus? RemittanceStatus { get; set; }
    }

    public class ListTransactionHistoryModel : PagingModel
    {
        public List<TransactionHistoryModel> ListTransactionHistory { get; set; }

        public int TotalCount { get; set; }

        public ListTransactionHistoryModel()
        {
            ListTransactionHistory = new List<TransactionHistoryModel>();
            TotalCount = 0;
        }
    }
}
