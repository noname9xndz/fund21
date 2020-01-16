using smartFunds.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace smartFunds.Data.Models
{
    public class TransactionHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public decimal CurrentAccountAmount { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal TotalWithdrawal { get; set; }
        public WithdrawalType? WithdrawalType { get; set; }
        public RemittanceStatus? RemittanceStatus { get; set; }
        public string Description { get; set; }
        public string ObjectId { get; set; }

    }
}
