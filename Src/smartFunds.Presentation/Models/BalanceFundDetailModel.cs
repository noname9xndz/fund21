using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Models
{
    public class BalanceFundDetailModel : PagingModel
    {
        public FundTransactionHistoryModel FundTransactionHistory { get; set; }

        public List<FundTransactionHistoryModel> ListFundTransactionHistoryModel { get; set; }

        public BalanceFundDetailModel()
        {
            ListFundTransactionHistoryModel = new List<FundTransactionHistoryModel>();
        }
    }
}
