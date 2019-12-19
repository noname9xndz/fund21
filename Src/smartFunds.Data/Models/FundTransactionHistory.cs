using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using smartFunds.Common;
using smartFunds.Common.Data.Repositories;

namespace smartFunds.Data.Models
{
    public class FundTransactionHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public int FundId { get; set; }
        [ForeignKey("FundId")]
        public Fund Fund { get; set; }
        public decimal NoOfCertificates { get; set; }
        public decimal TotalInvestNoOfCertificates { get; set; }
        public decimal TotalWithdrawnNoOfCertificates { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDate { get; set; }
        public EditStatus Status { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
