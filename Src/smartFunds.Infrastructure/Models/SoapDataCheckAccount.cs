using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Infrastructure.Models
{
    public class SoapDataCheckAccount
    {
        public string username { get; set; }
        public string password { get; set; }
        public string serviceCode { get; set; }
        public string orderId { get; set; }
        public string data { get; set; }
    }
}
