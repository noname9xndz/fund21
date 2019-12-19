using Microsoft.AspNetCore.Mvc;
using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace smartFunds.Model.Common
{
    public class FundTransactionHistoryModel
    {

        public int Id { get; set; }

        public UserModel User { get; set; }

        public FundModel Fund { get; set; }

        [Display(Name = "TransactionValue", ResourceType = typeof(Resources.Common))]
        public decimal NoOfCertificates { get; set; }

        [Display(Name = "TotalInvestAmount", ResourceType = typeof(Resources.Common))]
        public decimal TotalInvestNoOfCertificates { get; set; }

        [Display(Name = "TotalWithdrawnAmount", ResourceType = typeof(Resources.Common))]
        public decimal TotalWithdrawnNoOfCertificates { get; set; }

        [Display(Name = "TransactionDate", ResourceType = typeof(Resources.Common))]
        public DateTime TransactionDate { get; set; }

        [Display(Name = "Type", ResourceType = typeof(Resources.Common))]
        public TransactionType TransactionType { get; set; }

        public EditStatus Status { get; set; }
    }

    public class ListFundTransactionHistoryModel : PagingModel
    {
        public List<FundTransactionHistoryModel> ListFundTransactionHistory { get; set; }

        public int TotalCount { get; set; }

        public ListFundTransactionHistoryModel()
        {
            ListFundTransactionHistory = new List<FundTransactionHistoryModel>();
            TotalCount = 0;
        }
    }
}
