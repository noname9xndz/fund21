using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using smartFunds.Common.Data.Repositories;

namespace smartFunds.Data.Models
{
    public class KVRR : IPersistentEntity, ITrackedEntity
    {
        public KVRR()
        {
            KVRRPortfolios = new List<KVRRPortfolio>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public string KVRRImagePath { get; set; }
        public bool IsDeleted { get; set; }
        [MaxLength(30)]
        public string DeletedAt { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        [NotMapped]
        public IFormFile KVRRImage { get; set; }
        [NotMapped]
        public int EntityState { get; set; }
        [NotMapped]
        public List<string> PortfolioIds { get; set; }
        [NotMapped]
        public string PortfolioId { get; set; }
        public virtual List<KVRRPortfolio> KVRRPortfolios { get; set; }
    }
}
