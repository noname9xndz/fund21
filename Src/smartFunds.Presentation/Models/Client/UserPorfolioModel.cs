using smartFunds.Model.Client;
using smartFunds.Model.Common;
using System.Collections.Generic;

namespace smartFunds.Presentation.Models.Client
{
    public class UserPorfolioModel
    {
        public UserModel  CurrentUser { get; set; }

        public List<UserPorfolio> UserPortfolios { get; set; }
    }
}
