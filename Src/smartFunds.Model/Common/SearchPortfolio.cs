using Microsoft.AspNetCore.Mvc.Rendering;
using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace smartFunds.Model.Common
{
    public class SearchPortfolio
    {
        [Display(Name = "PortfolioName", ResourceType = typeof(Resources.Common))]
        public string PortfolioName { get; set; }

        [Display(Name = "PortfolioContent", ResourceType = typeof(Resources.Common))]
        public string PortfolioContent { get; set; }

        [Display(Name = "KVRRName", ResourceType = typeof(Resources.Common))]
        public string KVRRName { get; set; }
    }
}
