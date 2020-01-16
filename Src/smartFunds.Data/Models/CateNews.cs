using smartFunds.Common.Data.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using smartFunds.Common;

namespace smartFunds.Data.Models
{
    public  class CateNews: ITrackedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
      
        public int ParentNewsID
        {
            get;
            set;
        }

        [Column(TypeName = "NVARCHAR(500)")]
        public string CateNewsName
        {
            get;
            set;
        }

       
        public int CateNewsOrder
        {
            get;
            set;
        }

       
       
        public DateTime Created
        {
            get;
            set;
        }

      
        
    
        public bool Status
        {
            get;
            set;
        }

        [Column(TypeName = "NVARCHAR(1000)")]
        public string Contents
        {
            get;
            set;
        }

        [Column(TypeName = "VARCHAR(500)")]
        public string Images
        {
            get;
            set;
        }
     
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }

    }
}
