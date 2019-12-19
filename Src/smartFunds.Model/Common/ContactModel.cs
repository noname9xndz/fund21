using System;
using System.ComponentModel.DataAnnotations;
using static smartFunds.Common.Constants;

namespace smartFunds.Model.Common
{
    public class ContactModel
    {
        public int Id { get; set; }

        [Display(Name = "FullName", ResourceType = typeof(Resources.Common))]
        [Required(ErrorMessage = Message.FieldEmpty)]
        public string FullName { get; set; }

        [Display(Name = "Content", ResourceType = typeof(Resources.Common))]
        [Required(ErrorMessage = Message.FieldEmpty)]
        public string Content { get; set; }

        [Display(Name = "PhoneNumber", ResourceType = typeof(Resources.Common))]
        [RegularExpression(@"^(0[0-9]{9})$", ErrorMessageResourceName = "FieldFormat", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        public string PhoneNumber { get; set; }

        [Display(Name = "EmailAddress", ResourceType = typeof(Resources.Common))]
        [EmailAddress(ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "FieldFormat")]
        [Required(ErrorMessage = Message.FieldEmpty)]
        public string Email { get; set; }

        [Display(Name = "DateLastUpdated", ResourceType = typeof(Resources.Common))]
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
