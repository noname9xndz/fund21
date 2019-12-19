using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Models
{
    public class ActiveViewModel
    {
        public string UserName { get; set; }

        [Required]
        public string VerifyCode { get; set; }
    }
}
