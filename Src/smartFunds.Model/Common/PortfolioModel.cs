using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smartFunds.Common;

namespace smartFunds.Model.Common
{
    public class PortfolioModel
    {
        public PortfolioModel()
        {
            KVRRPortfolios = new List<KVRRPortfolioModel>();
            PortfolioFunds = new List<PortfolioFundModel>();
        }

        public int Id { get; set; }

        [Display(Name = "PortfolioName", ResourceType = typeof(Resources.Common))]
        [Required(ErrorMessage = Constants.Message.FieldEmpty)]
        [Remote("IsPortfolioNameExists", "Portfolio", HttpMethod = "POST", ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "IsPortfolioNameExists", AdditionalFields = "initTitle")]
        public string Title { get; set; }

        [Display(Name = "Content", ResourceType = typeof(Resources.Common))]
        public string Content { get; set; }

        [Display(Name = "DateLastUpdated", ResourceType = typeof(Resources.Common))]
        public DateTime DateLastUpdated { get; set; }

        public string LastUpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedAt { get; set; }
        public virtual List<KVRRPortfolioModel> KVRRPortfolios { get; set; }        
        public virtual List<PortfolioFundModel> PortfolioFunds { get; set; }
        [NotMapped]
        public string IsFundPercent100 { get; set; }
    }

    public class PortfoliosModel : PagingModel
    {
        public PortfoliosModel()
        {
            Portfolios = new List<PortfolioModel>();
        }

        public List<PortfolioModel> Portfolios { get; set; }
    }
}

