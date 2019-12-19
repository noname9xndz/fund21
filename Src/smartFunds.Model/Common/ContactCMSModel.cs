using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace smartFunds.Model.Common
{
    public class ContactCMSModel
    {
        public int Id { get; set; }

        [Display(Name = "EmailAddress", ResourceType = typeof(Model.Resources.Common))]
        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [EmailAddress(ErrorMessageResourceName = "FieldFormat", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        public string Email { get; set; }

        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [Display(Name = "HotLine", ResourceType = typeof(Model.Resources.Common))]
        public string Phone { get; set; }

        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [EmailAddress(ErrorMessageResourceName = "FieldFormat", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [Display(Name = "EmailForReceiving", ResourceType = typeof(Resources.Common))]
        public string EmailForReceiving { get; set; }
    }
}
