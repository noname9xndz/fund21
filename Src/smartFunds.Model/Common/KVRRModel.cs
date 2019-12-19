using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace smartFunds.Model.Common
{
    public class KVRRModel
    {
        public KVRRModel()
        {
            KVRRPortfolios = new List<KVRRPortfolioModel>();
        }

        [Required]
        public int Id { get; set; }
        [Required(ErrorMessageResourceName = "KVRRNameIsNotEmpty", ErrorMessageResourceType = typeof(Resources.ValidationMessages))]
        [Display(Name = "KVRRName", ResourceType = typeof(Resources.Common))]
        [Remote("IsDuplicateName", "KVRR", HttpMethod = "POST", ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "IsKVRRNameExists", AdditionalFields = "initName")]
        public string Name { get; set; }
        [Required(ErrorMessageResourceName = "KVRRDetailIsNotEmpty", ErrorMessageResourceType = typeof(Resources.ValidationMessages))]
        [Display(Name = "Content", ResourceType = typeof(Resources.Common))]
        public string Detail { get; set; }
        public string KVRRImagePath { get; set; }
        public bool IsDeleted { get; set; }
        [MaxLength(30)]
        public string DeletedAt { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        [NotMapped]
        [Display(Name = "KVRRImage", ResourceType = typeof(Resources.Common))]        
        public IFormFile KVRRImage { get; set; }
        [NotMapped]
        public int EntityState { get; set; }
        [NotMapped]
        public List<string> PortfolioIds { get; set; }
        [NotMapped]
        [Required(ErrorMessageResourceName = "PortfolioIsNotAvailable", ErrorMessageResourceType = typeof(Resources.ValidationMessages))]
        public string PortfolioId { get; set; }
        public virtual List<KVRRPortfolioModel> KVRRPortfolios { get; set; }
    }

    public class KVRRsModel : PagingModel
    {
        public KVRRsModel()
        {
            KVRRs = new List<KVRRModel>();
        }
        public List<KVRRModel> KVRRs { get; set; }
    }
}
