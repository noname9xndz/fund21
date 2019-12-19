using Microsoft.AspNetCore.Mvc;
using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace smartFunds.Model.Common
{
    public class InvestmentTargetModel
    {
        public int Id { get; set; }

        public UserModel User { get; set; }

        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        [Display(Name = "ExpectedAmount", ResourceType = typeof(Resources.Common))]
        public decimal? ExpectedAmount { get; set; }

        [Display(Name = "Duration", ResourceType = typeof(Resources.Common))]
        public Duration Duration { get; set; }

        [Display(Name = "Frequency", ResourceType = typeof(Resources.Common))]
        public Frequency Frequency { get; set; }

        [Display(Name = "OneTimeAmount", ResourceType = typeof(Resources.Common))]
        public decimal OneTimeAmount { get; set; }

        [Display(Name = "InvestmentMethod", ResourceType = typeof(Resources.Common))]
        public InvestmentMethod InvestmentMethod { get; set; }

        public EditStatus Status { get; set; }

        public DateTime DateLastUpdated { get; set; }

        public string LastUpdatedBy { get; set; }

        public bool IsDeleted { get; set; }

        public string DeletedAt { get; set; }

        [NotMapped]
        [Display(Name = "InvestmentStatus", ResourceType = typeof(Resources.Common))]
        public string InvestmentStatus { get; set; }

        [NotMapped]
        [Display(Name = "InvestmentAmount", ResourceType = typeof(Resources.Common))]
        public decimal InvestmentAmount { get; set; }

        [NotMapped]
        [Display(Name = "InvestmentDuration", ResourceType = typeof(Resources.Common))]
        public int InvestmentDuration { get; set; }
    }
}
