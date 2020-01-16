using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Models
{
    public class InvestViewModel
    {
        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [Display(Name = "TargetAmount", ResourceType = typeof(Model.Resources.Common))]
        [Range(typeof(decimal), "1000", "79228162514264337593543950335", ErrorMessage = "Số tiền đầu tư phải lớn hơn hoặc bằng 1000 VNĐ")]
        public decimal? TargetAmount { get; set; }
    }
}
