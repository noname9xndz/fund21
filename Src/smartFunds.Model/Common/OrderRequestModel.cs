using System;
using System.ComponentModel.DataAnnotations;
using smartFunds.Common;

namespace smartFunds.Model.Common
{
    public class OrderRequestModel
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
