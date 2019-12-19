using smartFunds.Common.Data.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using smartFunds.Common;

namespace smartFunds.Model.Common
{
    public class KVRRMarkModel : ITrackedEntity
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "MarkFromIsNotEmpty")]
        [Display(Name = "MarkFrom", ResourceType = typeof(Resources.Common))]
        [RegularExpression("^[0-9]+$", ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "MarkMustBeNumber")]
        [Range(0, int.MaxValue, ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "MarkFromRange")]        
        public int MarkFrom { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "MarkToIsNotEmpty")]
        [Display(Name = "MarkTo", ResourceType = typeof(Resources.Common))]
        [RegularExpression("^[0-9]+$", ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "MarkMustBeNumber")]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "MarkToRange")]        
        public int MarkTo { get; set; }

        [Remote("KVRRIsNotEmpty", "KVRRMark", HttpMethod = "POST", ErrorMessageResourceType = typeof(Resources.ValidationMessages), ErrorMessageResourceName = "KVRRIsNotEmpty", AdditionalFields = "initKvrrId")]
        public int KVRRId { get; set; }
        [NotMapped]
        public KVRRModel KVRR { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        [NotMapped]
        public int ValidMark { get; set; }
        [NotMapped]
        public FormState EntityState { get; set; }
    }

    public class KVRRMarksModel : PagingModel
    {
        public KVRRMarksModel()
        {
            KVRRMarks = new List<KVRRMarkModel>();
        }
        public List<KVRRMarkModel> KVRRMarks { get; set; }
    }
}
