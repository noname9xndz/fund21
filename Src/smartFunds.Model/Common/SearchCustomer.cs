using Microsoft.AspNetCore.Mvc.Rendering;
using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static smartFunds.Common.Constants;

namespace smartFunds.Model.Common
{
    public class SearchCustomer
    {
        [Display(Name = "FullName", ResourceType = typeof(Resources.Common))]
        public string FullName { get; set; }

        [Display(Name = "PhoneNumber", ResourceType = typeof(Resources.Common))]
        public string PhoneNumber { get; set; }

        [Display(Name = "EmailAddress", ResourceType = typeof(Resources.Common))]
        public string Email { get; set; }

        [Display(Name = "AccountCreated", ResourceType = typeof(Resources.Common))]
        public string CreatedDate { get; set; }

        [Display(Name = "ActiveStatus", ResourceType = typeof(Resources.Common))]
        public ActiveStatus ActiveStatus { get; set; }

        public List<SelectListItem> Status { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = ActiveStatus.None.ToString(), Text = Resources.Common.All },
            new SelectListItem { Value = ActiveStatus.Active.ToString(), Text = Resources.Common.Active },
            new SelectListItem { Value = ActiveStatus.Inactive.ToString(), Text = Resources.Common.Inactive }
        };
    }
}
