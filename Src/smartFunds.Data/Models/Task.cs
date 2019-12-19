using smartFunds.Common;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartFunds.Data.Models
{
    public class AdminTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public TransactionTypeAdmin TransactionType { get; set; }        
        public Fund Fund { get; set; }
        public int FundId { get; set; }
        public decimal TransactionAmount { get; set; }            
        public TransactionStatus Status { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
