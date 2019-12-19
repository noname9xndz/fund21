using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace smartFunds.Data.Models
{
    public class HomepageCMS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string Category { get; set; }
        [NotMapped]
        public IFormFile Banner { get; set; }
        [NotMapped]
        public List<HomepageCMS> HomepageModels { get; set; }
    }
}
