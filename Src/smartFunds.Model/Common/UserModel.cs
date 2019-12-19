using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace smartFunds.Model.Common
{
    public class UserModel : IdentityUser
    {
        [Required]
        [Display(Name = "FullName", ResourceType = typeof(Resources.Common))]
        public string FullName { get; set; }

        [Display(Name = "InitialInvestmentAmount", ResourceType = typeof(Resources.Common))]
        public decimal InitialInvestmentAmount { get; set; }

        [Display(Name = "CurrentAccountAmount", ResourceType = typeof(Resources.Common))]
        public decimal CurrentAccountAmount { get; set; }

        [Display(Name = "AccountCreated", ResourceType = typeof(Resources.Common))]
        public DateTime Created { get; set; }

        public DateTime DateLastUpdated { get; set; }

        public string LastUpdatedBy { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public string DeletedAt { get; set; }
    }
}
