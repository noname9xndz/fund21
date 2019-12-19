using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Models
{
    public class OrderVerifyModel
    {
        public string billcode { get; set; }
        public string error_code { get; set; }
        public string merchant_code { get; set; }
        public string order_id { get; set; }
        public string trans_amount { get; set; }
        public string check_sum { get; set; }
    }
}
