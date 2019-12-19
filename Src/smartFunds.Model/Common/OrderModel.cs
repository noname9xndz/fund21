using System.ComponentModel.DataAnnotations;

namespace smartFunds.Model.Common
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string Desc { get; set; }
        public string MerchantCode { get; set; }
        public string Msisdn { get; set; }
        public string TransAmount { get; set; }
        public string Version { get; set; }
        public bool IsInvestmentTarget { get; set; }
        public string CheckSum { get; set; }
        public bool IsSuccess { get; set; }
    }
}
