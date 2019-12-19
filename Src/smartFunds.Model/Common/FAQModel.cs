using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace smartFunds.Model.Common
{
    public class FAQModel
    {
        public int Id { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Common))]
        public string Title { get; set; }

        [Display(Name = "Content", ResourceType = typeof(Resources.Common))]
        public string Content { get; set; }

        [Display(Name = "DateLastUpdated", ResourceType = typeof(Resources.Common))]
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public List<FAQModel> RelatedFAQs { get; set; }
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
