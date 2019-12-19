using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using smartFunds.Common;

namespace smartFunds.Model.Common
{
    public class PortfolioFundModel
    {
        public int PortfolioId { get; set; }
        public PortfolioModel Portfolio { get; set; }
        public int FundId { get; set; }
        public FundModel Fund { get; set; }        
        [RegularExpression(@"^[-+]?[0-9]+(,[0-9]{3})*(\.[0-9]+)?([eE][-+]?[0-9]+)?", ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "FundPercentMustBeNumber")]
        public decimal? FundPercent { get; set; }

        public decimal? FundPercentNew { get; set; }
        public EditStatus EditStatus { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
