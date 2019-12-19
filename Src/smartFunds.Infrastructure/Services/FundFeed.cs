using smartFunds.Model.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace smartFunds.Infrastructure.Services
{
    public interface IFundFeed
    {
        Task<List<FundModel>> GetFunds();
    }
    public class FundFeed : IFundFeed
    {
        public async Task<List<FundModel>> GetFunds()
        {       
            List<FundModel> funds = new List<FundModel>();
            //to do: get funds from external API
            return funds;
        }
    }
}
