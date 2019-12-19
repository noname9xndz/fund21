using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Data.Models
{
    public class OrderRequest
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string ErrorCode { get; set; }
    }
}
