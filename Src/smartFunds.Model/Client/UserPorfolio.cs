using smartFunds.Common;

namespace smartFunds.Model.Client
{
    public class UserPorfolio
    {
        public decimal? CurrentAccountAmount { get; set; }

        public string FundName { get; set; }

        public string FundCode { get; set; }

        public decimal? CertificateValue { get; set; }

        public decimal? OldCertificateValue { get; set; }

        public decimal? NoOfCertificates { get; set; }
        
        public EditStatus Status { get; set; }
    }
}
