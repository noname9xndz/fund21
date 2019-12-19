using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Models
{
    public class PaymentResultModel
    {
        public string error_code { get; set; }
        public string merchant_code { get; set; }
        public string order_id { get; set; }
        public string return_url { get; set; }
        public string return_bill_code { get; set; }
        public string return_other_info { get; set; }
        public string check_sum { get; set; }
    }
}
