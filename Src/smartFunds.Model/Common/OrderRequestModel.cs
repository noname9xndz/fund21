using System.ComponentModel.DataAnnotations;

namespace smartFunds.Model.Common
{
    public class OrderRequestModel
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        public string ErrorCode { get; set; }
    }
}
