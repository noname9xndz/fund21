using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Infrastructure.Models
{
    public class SoapDataRequestPayment : SoapData
    {
        public string totalAmount { get; set; }
        public string totalTrans { get; set; }
        public string transContent { get; set; }
        public string data { get; set; }
    }
}
