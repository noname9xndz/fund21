using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Models
{
    public class CheckFundViewModel
    {
        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [Display(Name = "Fund", ResourceType = typeof(Model.Resources.Common))]
        public string FundId { get; set; }

        public int CountUserFund { get; set; }

        public decimal TotalAmountUserFund { get; set; }
    }
}
