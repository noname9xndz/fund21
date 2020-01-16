using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static smartFunds.Common.Constants;
using smartFunds.Common;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using System;

namespace smartFunds.Model.Common
{
    public class CateNewsModel
    {
        public int Id { get; set; }

        [Display(Name = "CateTitle", ResourceType = typeof(Resources.Common))]
        [Required(ErrorMessage = Message.FieldEmpty)]
        public string CateNewsName { get; set; }

        public int ParentNewsID { get; set; }
        [Display(Name = "CateDescript", ResourceType = typeof(Resources.Common))]
       
        public string Contents { get; set; }


        [Display(Name = "CateOrder", ResourceType = typeof(Resources.Common))]
        [Required(ErrorMessage = Message.FieldEmpty)]
        public int CateNewsOrder { get; set; }

        [Display(Name = "CateImg", ResourceType = typeof(Resources.Common))]
        public string Images { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Resources.Common))]
        [Required(ErrorMessage = Message.FieldEmpty)]
        public bool Status { get; set; }

        
        public DateTime Created { get; set; }
        [Display(Name = "DateLastUpdated", ResourceType = typeof(Resources.Common))]
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
      
    }

    public class CateNewssModel : PagingModel
    {
        public CateNewssModel()
        {
            CateNewss = new List<CateNewsModel>();
        }
        public List<CateNewsModel> CateNewss { get; set; }

    }
}
