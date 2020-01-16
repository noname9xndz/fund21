using smartFunds.Model.Client;
using smartFunds.Model.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Models
{
    public class DetailCustomerViewModel
    {
        public UserModel Customer { get; set; }

        public ListTransactionHistoryModel ListTransactionHistoryModel { get; set; }

        public PropertyFluctuations PropertyFluctuations { get; set; }

        public InvestmentTargetModel InvestmentTarget { get; set; }

        public List<UserPorfolio> UserPortfolios { get; set; }
    }
}
