using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Infrastructure.Models
{
    public class SoapDataRequestPayment
    {
        public string username { get; set; }
        public string password { get; set; }
        public string serviceCode { get; set; }
        public string orderId { get; set; }
        public string totalAmount { get; set; }
        public string totalTrans { get; set; }
        public string transContent { get; set; }
        public string data { get; set; }
    }
}
