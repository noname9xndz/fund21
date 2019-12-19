using Microsoft.AspNetCore.Mvc.Rendering;
using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace smartFunds.Model.Common
{
    public class SearchFund
    {
        [Display(Name = "FundName", ResourceType = typeof(Resources.Common))]
        public string FundName { get; set; }

        [Display(Name = "FundCode", ResourceType = typeof(Resources.Common))]
        public string FundCode { get; set; }

        [Display(Name = "FundContent", ResourceType = typeof(Resources.Common))]
        public string FundContent { get; set; }

        [Display(Name = "PortfolioName", ResourceType = typeof(Resources.Common))]
        public string PortfolioName { get; set; }
    }

    public class ListUpdateNav
    {
        public ListUpdateNav()
        {
            UpdateNav = new List<UpdateNav>();
        }
        public List<UpdateNav> UpdateNav { get; set; }
    }

    public class UpdateNav
    {
        public int ID { get; set; }
        public decimal NAVNew { get; set; }
    }
}
