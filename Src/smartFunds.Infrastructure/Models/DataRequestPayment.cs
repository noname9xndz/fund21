using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Infrastructure.Models
{
    public class DataRequestPayment
    {
        public string transId { get; set; }
        public string msisdn { get; set; }
        public string customerName { get; set; }
        public string amount { get; set; }
        public string smsContent { get; set; }
        public string note { get; set; }
    }
}
