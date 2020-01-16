using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using smartFunds.Common;

namespace smartFunds.Model.Common
{
    public class KVRRQuestion
    {
        public KVRRQuestion()
        {
            KVRRAnswers = new List<KVRRAnswer>();
        }
        [Required]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public string ImageDesktop { get; set; }
        public string ImageMobile { get; set; }
        public List<KVRRAnswer> KVRRAnswers { get; set; }
        public int No { get; set; }
        [Display(Name = "Category", ResourceType = typeof(Resources.Common))]
        public KVRRQuestionCategories KVRRQuestionCategories { get; set; }

        [NotMapped]
        [Required]
        public string AnswerSelected { get; set; }
    }
}
