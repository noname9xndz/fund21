using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Models
{
    public class NewUserViewModel
    {
        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [Display(Name = "FullName", ResourceType = typeof(Model.Resources.Common))]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [Display(Name = "EmailAddress", ResourceType = typeof(Model.Resources.Common))]
        [EmailAddress(ErrorMessageResourceName = "FieldFormat", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [Display(Name = "Role", ResourceType = typeof(Model.Resources.Common))]
        public string Role { get; set; }

        public List<SelectListItem> Roles { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = RoleName.Admin, Text = Model.Resources.Common.Admin },
            new SelectListItem { Value = RoleName.CustomerManager, Text = Model.Resources.Common.CustomerService },
            new SelectListItem { Value = RoleName.InvestmentManager, Text = Model.Resources.Common.BackOfficer },
            new SelectListItem { Value = RoleName.Accountant, Text = Model.Resources.Common.Accountant }
        };

        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[0-9])(?=.*[#?!@$%^&*-]).{8,}$", ErrorMessageResourceName = "PasswordInvalid", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Model.Resources.Common))]
        public string Password { get; set; }
    }
}
