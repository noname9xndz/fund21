using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        public string EmailOrPhone { get; set; }
    }
}
