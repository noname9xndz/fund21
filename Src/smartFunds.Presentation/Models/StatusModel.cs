using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Models
{
    public class StatusModel
    {
        public string Status { get; set; }
        public string smartFundsDatabase { get; set; }      
        public string smartFundsContactBaseDatabase { get; set; }
        public string Version { get; set; }      
    }
}
