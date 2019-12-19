using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [RegularExpression(@"^(0[0-9]{9})$", ErrorMessageResourceName = "FieldFormat", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [Display(Name = "PhoneNumber", ResourceType = typeof(Model.Resources.Common))]
        public string UserName { get; set; }

        //[Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        //[RegularExpression(@"^(?=.*[a-z])(?=.*[0-9])(?=.*[#?!@$%^&*-]).{8,}$", ErrorMessageResourceName = "PasswordInvalid", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password", ResourceType = typeof(Model.Resources.Common))]
        //public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(Model.Resources.Common))]
        public bool RememberMe { get; set; }
    }
}
