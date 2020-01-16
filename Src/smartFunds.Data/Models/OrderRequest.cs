using System;
using System.Collections.Generic;
using System.Text;
using smartFunds.Common;

namespace smartFunds.Data.Models
{
    public class OrderRequest
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string ErrorCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { set; get; }
        public string UpdatedBy { set; get; }
        public decimal Amount { set; get; }
        public OrderRequestStatus Status { set; get; }
    }
}
