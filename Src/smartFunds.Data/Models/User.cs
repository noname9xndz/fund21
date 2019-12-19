using Microsoft.AspNetCore.Identity;
using smartFunds.Common.Data.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartFunds.Data.Models
{
    public class User : IdentityUser, IPersistentEntity, ITrackedEntity
    {
        [PersonalData]
        public string FullName { get; set; }
        [PersonalData]
        public decimal InitialInvestmentAmount { get; set; }
        [Column(TypeName = "decimal(15,7)")]
        public decimal AdjustmentFactor { get; set; }
        public decimal CurrentInvestmentAmount { get; set; }
        [PersonalData]
        public decimal CurrentAccountAmount { get; set; }
        [PersonalData]
        public decimal AmountWithdrawn { get; set; }
        public decimal CurrentAmountWithdrawn { get; set; }
        [PersonalData]
        [ForeignKey("KVRRId")]
        public KVRR KVRR { get; set; }
        public int? KVRRId { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedAt { get; set; }
        public ICollection<TransactionHistory> TransactionHistory { get; set; }        
        public virtual List<UserFund> UserFunds { get; set; }
        public bool WithdrawProcessing { get; set; }
        public DateTime WithdrawProcessingDate { get; set; }
    }
}
