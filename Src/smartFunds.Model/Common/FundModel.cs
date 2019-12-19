using Microsoft.AspNetCore.Mvc;
using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace smartFunds.Model.Common
{
    public class FundModel
    {
        public FundModel()
        {
            PortfolioFunds = new List<PortfolioFundModel>();
        }

        public int Id { get; set; }
        [Display(Name = "FundName", ResourceType = typeof(Resources.Common))]
        [Required(ErrorMessageResourceName = "FundNameIsNotEmpty", ErrorMessageResourceType = typeof(Resources.ValidationMessages))]
        [Remote("IsDuplicateName", "Fund", HttpMethod = "POST", ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "IsFundNameExists", AdditionalFields = "initName")]
        public string Title { get; set; }
        [Display(Name = "FundContent", ResourceType = typeof(Resources.Common))]
        public string Content { get; set; }
        [Display(Name = "FundCode", ResourceType = typeof(Resources.Common))]
        [Required(ErrorMessageResourceName = "FundCodeIsNotEmpty", ErrorMessageResourceType = typeof(Resources.ValidationMessages))]
        [Remote("IsDuplicateCode", "Fund", HttpMethod = "POST", ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "IsFundCodeExists", AdditionalFields = "initCode")]
        public string Code { get; set; }
        [Display(Name = "FundLink", ResourceType = typeof(Resources.Common))]
        public string Link { get; set; }
        [Display(Name = "DateLastUpdated", ResourceType = typeof(Resources.Common))]
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedAt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0,0.00}")]
        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        public decimal NAV { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0,0.00}")]
        public decimal NAVNew { get; set; }
        public DateTime DateLastApproved { get; set; }
        public EditStatus EditStatus { get; set; }
        public virtual List<PortfolioFundModel> PortfolioFunds { get; set; }
    }

    public class FundsModel : PagingModel
    {
        public FundsModel()
        {
            Funds = new List<FundModel>();
        }
        public List<FundModel> Funds { get; set; }
    }
}
