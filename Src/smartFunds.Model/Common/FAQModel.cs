using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static smartFunds.Common.Constants;
using smartFunds.Common;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace smartFunds.Model.Common
{
    public class FAQModel
    {
        public int Id { get; set; }

        [Display(Name = "QuestionContent", ResourceType = typeof(Resources.Common))]
        [Required(ErrorMessage = Message.FieldEmpty)]
        public string Title { get; set; }

        [Display(Name = "AnswerContent", ResourceType = typeof(Resources.Common))]
        [Required(ErrorMessage = Message.FieldEmpty)]
        public string Content { get; set; }

        [Display(Name = "DateLastUpdated", ResourceType = typeof(Resources.Common))]
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        [Display(Name = "Category", ResourceType = typeof(Resources.Common))]
        public FAQCategory Category { get; set; }
    }

    public class FAQsModel : PagingModel
    {
        public FAQsModel()
        {
            FAQs = new List<FAQModel>();
        }
        public List<FAQModel> FAQs { get; set; }
        
    }
}
