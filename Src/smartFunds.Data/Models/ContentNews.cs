using smartFunds.Common.Data.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using smartFunds.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace smartFunds.Data.Models
{
   public class ContentNews : ITrackedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        


       
        public int CateNewsID
        {
            get;
            set;
        }
        [NotMapped]
        public IFormFile NewsImage { get; set; }
        [NotMapped]

        public string TransactionDateFrom
        {
            get;
            set;
        }
        [NotMapped]
        public List<SelectListItem> CateNewsIDs { get; set; }

        [Column(TypeName = "NVARCHAR(500)")]
        public string Title
        {
            get;
            set;
        }

        [Column(TypeName = "NVARCHAR(500)")]
        public string ShortDescribe
        {
            get;
            set;
        }
        [MaxLength, Column(TypeName = "ntext")]
        public string Contents
        {
            get;
            set;
        }

        [Column(TypeName = "NVARCHAR(500)")]
        public string ImageThumb
        {
            get;
            set;
        }

        [Column(TypeName = "NVARCHAR(500)")]
        public string ImageLarge
        {
            get;
            set;
        }

        [Column(TypeName = "NVARCHAR(250)")]
        public string FileName
        {
            get;
            set;
        }

        [Column(TypeName = "NVARCHAR(200)")]
        public string Author
        {
            get;
            set;
        }


        public System.DateTime PostDate
        {
            get;
            set;
        }

      

    
        public bool Status
        {
            get;
            set;
        }

       

       
        public bool Ishome
        {
            get;
            set;
        }


        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
