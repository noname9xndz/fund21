using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Models
{
    public class UpdateProfileViewModel
    {
        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [Display(Name = "FullName", ResourceType = typeof(Model.Resources.Common))]
        public string FullName { get; set; }

        [Display(Name = "FullName", ResourceType = typeof(Model.Resources.Common))]
        public string Email { get; set; }

        public string RoleName { get; set; }
    }
}
