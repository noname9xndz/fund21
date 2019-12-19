using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Infrastructure.Models
{
    public class SMSConfig
    {
        public string APIKey { get; set; }
        public string SecretKey { get; set; }
        public bool IsFlash { get; set; }
        public string BrandName { get; set; }
        public int SMSType { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
    }
}
