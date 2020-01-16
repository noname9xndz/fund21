using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static smartFunds.Common.Constants;
using smartFunds.Common;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace smartFunds.Model.Common
{
   public class ContentNewsModel
    {

        public int Id { get; set; }

        [Display(Name = "CateNews", ResourceType = typeof(Resources.Common))]
        [Required(ErrorMessage = Message.FieldEmpty)]

        public int CateNewsID { get; set; }

        public List<SelectListItem> CateNewsIDs { get; set; }


        [Display(Name = "NewsTitle", ResourceType = typeof(Resources.Common))]
        [Required(ErrorMessage = Message.FieldEmpty)]
        public string Title { get; set; }

       
        public string TransactionDateFrom { get; set; }
        [Display(Name = "NewsDescript", ResourceType = typeof(Resources.Common))]

        public string ShortDescribe { get; set; }

        [Display(Name = "NewsContent", ResourceType = typeof(Resources.Common))]
   

       
        public string Contents { get; set; }
        public IFormFile NewsImage { get; set; }
        public string ImageThumb { get; set; }

       // public string ImageThumbPath { get; set; }

        public string Author { get; set; }
        public bool Status { get; set; }
        public string FileName { get; set; }
        public string ImageLarge { get; set; }
        public bool Ishome { get; set; }
        [Display(Name = "NewsPostDate", ResourceType = typeof(Resources.Common))]
        public DateTime PostDate { get; set; }

        [Display(Name = "DateLastUpdated", ResourceType = typeof(Resources.Common))]
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
 
    }
    public class LstNewsModel : PagingModel
    {
        public int TotalCount { get; set; }

        public LstNewsModel()
        {
            LstNews = new List<ContentNewsModel>();
            TotalCount = 0;
        }
        public List<ContentNewsModel> LstNews { get; set; }

    }
}
