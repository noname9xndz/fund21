using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Models
{
    public class EditCustomerViewModel
    {
        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        public string CustomerId { get; set; }

        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [Display(Name = "FullName", ResourceType = typeof(Model.Resources.Common))]
        public string FullName { get; set; }

        [RegularExpression(@"^(0[0-9]{9})$", ErrorMessageResourceName = "FieldFormat", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [Display(Name = "PhoneNumber", ResourceType = typeof(Model.Resources.Common))]
        public string PhoneNumber { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastLogin { get; set; }
    }
}
