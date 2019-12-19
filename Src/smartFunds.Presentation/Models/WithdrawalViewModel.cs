using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Models
{
    public class WithdrawalViewModel
    {
        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [Display(Name = "WithdrawalAmount", ResourceType = typeof(Model.Resources.Common))]
        public decimal? WithdrawalAmount { get; set; }

        [Display(Name = "WithdrawalType", ResourceType = typeof(Model.Resources.Common))]
        public WithdrawalType WithdrawalType { get; set; }

        [Display(Name = "WithdrawalFee", ResourceType = typeof(Model.Resources.Common))]
        public decimal WithdrawalFee { get; set; }
    }
}
