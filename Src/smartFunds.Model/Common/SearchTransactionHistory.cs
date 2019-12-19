using Microsoft.AspNetCore.Mvc.Rendering;
using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace smartFunds.Model.Common
{
    public class SearchTransactionHistory
    {
        [Display(Name = "CustomerName", ResourceType = typeof(Resources.Common))]
        public string CustomerName { get; set; }

        [Display(Name = "PhoneNumber", ResourceType = typeof(Resources.Common))]
        public string PhoneNumber { get; set; }

        [Display(Name = "EmailAddress", ResourceType = typeof(Resources.Common))]
        public string EmailAddress { get; set; }

        [Display(Name = "Type", ResourceType = typeof(Resources.Common))]
        public TransactionType TransactionType { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Resources.Common))]
        public TransactionStatus Status { get; set; }

        public string AmountFrom { get; set; }

        public string AmountTo { get; set; }

        public string TransactionDateFrom { get; set; }

        public string TransactionDateTo { get; set; }
    }
}
