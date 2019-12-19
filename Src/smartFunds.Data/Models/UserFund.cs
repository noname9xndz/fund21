using smartFunds.Common;

namespace smartFunds.Data.Models
{
    public class UserFund
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int FundId { get; set; }
        public Fund Fund { get; set; }
        public decimal? NoOfCertificates { get; set; }
        public EditStatus EditStatus { get; set; }
    }
}
