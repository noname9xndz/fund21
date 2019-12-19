using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using smartFunds.Common.Data.Repositories;

namespace smartFunds.Data.Models
{
    public class Portfolio : IPersistentEntity, ITrackedEntity
    {
        public Portfolio()
        {
            KVRRPortfolios = new List<KVRRPortfolio>();
            PortfolioFunds = new List<PortfolioFund>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedAt { get; set; }
        public virtual List<KVRRPortfolio> KVRRPortfolios { get; set; }
        public virtual List<PortfolioFund> PortfolioFunds { get; set; }
    }
}
