using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static smartFunds.Common.Constants;

namespace smartFunds.Presentation.Models
{
    public class ListInvestmentTargetSettingModel
    {
        public ListInvestmentTargetSettingModel()
        {
            ListModels = new List<InvestmentTargetSettingModel>();
        }
        public List<InvestmentTargetSettingModel> ListModels { get; set; }

    }

    public class ListInvestTargetDataModel
    {
        public ListInvestTargetDataModel()
        {
            ListModels = new List<InvestTargetData>();
        }
        public List<InvestTargetData> ListModels { get; set; }

    }

    public class InvestTargetData
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
    }
}
