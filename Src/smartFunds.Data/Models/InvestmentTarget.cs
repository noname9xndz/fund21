using smartFunds.Common;
using smartFunds.Common.Data.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace smartFunds.Data.Models
{
    public class InvestmentTarget : IPersistentEntity, ITrackedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        public decimal ExpectedAmount { get; set; }
        public Duration Duration { get; set; }
        public Frequency Frequency { get; set; }
        public decimal OneTimeAmount { get; set; }
        public InvestmentMethod InvestmentMethod { get; set; }
        public EditStatus Status { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedAt { get; set; }
    }
}
