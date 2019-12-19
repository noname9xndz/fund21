using Microsoft.AspNetCore.Mvc.Rendering;
using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace smartFunds.Model.Common
{
    public class SearchBalanceFund
    {
        public EditStatus Status { get; set; }

        public string AmountFrom { get; set; }

        public string AmountTo { get; set; }

        public int CustomerName { get; set; }

        public IEnumerable<FundModel> Funds { get; set; }

        public List<SelectListItem> TransactionTypeDropDownList
        {
            get
            {
                List<SelectListItem> selectList = new List<SelectListItem>();
                SelectListItem allItem = new SelectListItem
                {
                    Value = "0",
                    Text = Resources.Common.All
                };
                selectList.Add(allItem);
                SelectListItem buy = new SelectListItem
                {
                    Value = "1",
                    Text = Resources.Common.Buy
                };
                selectList.Add(buy);
                SelectListItem sell = new SelectListItem
                {
                    Value = "2",
                    Text = Resources.Common.Sell
                };
                selectList.Add(sell);
                return selectList;
            }
        }
    }
}
