using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace smartFunds.Model.Common
{
    public class UserModel : IdentityUser
    {
        public UserModel()
        {
            KVRROthers = new List<KVRRModel>();
        }

        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [Display(Name = "FullName", ResourceType = typeof(Resources.Common))]
        public string FullName { get; set; }

        [Display(Name = "InitialInvestmentAmount", ResourceType = typeof(Resources.Common))]
        public decimal InitialInvestmentAmount { get; set; }

        public decimal AdjustmentFactor { get; set; }

        public decimal CurrentInvestmentAmount { get; set; }

        [Display(Name = "CurrentAccountAmount", ResourceType = typeof(Resources.Common))]
        public decimal CurrentAccountAmount { get; set; }

        [Display(Name = "AmountWithdrawn", ResourceType = typeof(Resources.Common))]
        public decimal AmountWithdrawn { get; set; }

        public decimal CurrentAmountWithdrawn { get; set; }

        [Display(Name = "AccountCreated", ResourceType = typeof(Resources.Common))]
        public DateTime Created { get; set; }

        public DateTime LastLogin { get; set; }

        public DateTime DateLastUpdated { get; set; }

        public string LastUpdatedBy { get; set; }

        public bool IsDeleted { get; set; }

        public string DeletedAt { get; set; }
        public string SelectedKVRRId { get; set; }

        public KVRRModel KVRR { get; set; }
        [NotMapped]
        public List<KVRRModel> KVRROthers { get; set; }

        public bool WithdrawProcessing { get; set; }
        public DateTime WithdrawProcessingDate { get; set; }
    }
}
