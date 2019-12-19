using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static smartFunds.Common.Constants;
using smartFunds.Common;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

namespace smartFunds.Model.Common
{
    /*
     * Creator by PhuongNC
     */
    public class CustomerLevelModel
    {
        public int IDCustomerLevel { get; set; }

        [Display(Name = "NameCustomerLevel", ResourceType = typeof(Resources.Common))]
        public string NameCustomerLevel { get; set; }

        [Display(Name = "MinMoney", ResourceType = typeof(Resources.Common))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0,0}")]
        public decimal? MinMoney { get; set; }

        [Display(Name = "MaxMoney", ResourceType = typeof(Resources.Common))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0,0}")]
        public decimal? MaxMoney { get; set; }
    }

    public class CustomerLevelsModel : PagingModel
    {
        public CustomerLevelsModel()
        {
            CustomerLevels = new List<CustomerLevelModel>();
        }
        public List<CustomerLevelModel> CustomerLevels { get; set; }
    }
}
