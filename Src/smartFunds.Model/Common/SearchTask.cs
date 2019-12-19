using Microsoft.AspNetCore.Mvc.Rendering;
using smartFunds.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace smartFunds.Model.Common
{
    public class SearchTask
    {
        [Display(Name = "Status", ResourceType = typeof(Resources.Common))]
        public TransactionStatus Status { get; set; }

        public string AmountFrom { get; set; }

        public string AmountTo { get; set; }

        public TaskTypeAccountant CustomerName { get; set; }
        
        public IEnumerable<FundModel> Funds { get; set; }

        [Display(Name = "Fund", ResourceType = typeof(Resources.Common))]
        public List<SelectListItem> FundsDropDownList
        {
            get
            {
                List<SelectListItem> selectList = new List<SelectListItem>();
                if (Funds == null)
                {
                    return selectList;
                }
                SelectListItem allItem = new SelectListItem
                {
                    Value = "0",
                    Text = Resources.Common.All
                };
                selectList.Add(allItem);
                foreach (var fundModel in Funds)
                {
                    SelectListItem selectItem = new SelectListItem
                    {
                        Value = fundModel.Id.ToString(),
                        Text = fundModel.Title
                    };
                    selectList.Add(selectItem);
                }
                return selectList;
            }
        }
    }
}
