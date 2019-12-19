using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Models
{
    public class FundPurchaseFeeViewModel
    {
        public int Id { get; set; }
        public int FundId { get; set; }
        public FromLabel FromLabel { get; set; }
        public int From { get; set; }
        public ToLabel ToLabel { get; set; }
        public int To { get; set; }
        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [Display(Name = "FundPurchaseFee", ResourceType = typeof(Model.Resources.Common))]
        public decimal Fee { get; set; }
        public DecimalLabel DecimalFromLabel { get; set; }
        public DecimalLabel DecimalToLabel { get; set; }
    }
}
