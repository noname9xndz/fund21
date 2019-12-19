using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[0-9])(?=.*[#?!@$%^&*-]).{8,}$", ErrorMessageResourceName = "PasswordInvalid", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [DataType(DataType.Password)]
        [Display(Name = "NewPassword", ResourceType = typeof(Model.Resources.Common))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [DataType(DataType.Password)]
        [Display(Name = "NewPasswordConfirm", ResourceType = typeof(Model.Resources.Common))]
        [Compare("Password", ErrorMessageResourceName = "ConfirmPasswordInvalid", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
