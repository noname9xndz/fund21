using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using smartFunds.Common;
using smartFunds.Common.Data.Repositories;

namespace smartFunds.Data.Models
{
    public class Fund : IPersistentEntity, ITrackedEntity
    {
        public Fund()
        {
            PortfolioFunds = new List<PortfolioFund>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Code { get; set; }
        public string Link { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedAt { get; set; }
        public decimal NAV { get; set; }        
        public decimal NAVNew { get; set; }
        public DateTime DateLastApproved { get; set; }
        public EditStatus EditStatus { get; set; }
        public virtual List<PortfolioFund> PortfolioFunds  { get; set; }
        public virtual List<UserFund> UserFunds { get; set; }
    }
}
