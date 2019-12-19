using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace smartFunds.Model.Common
{
    public class HomepageCMSModel
    {
        
        public int Id { get; set; }
        public string ImageName { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public IFormFile Banner { get; set; }
        public List<HomepageCMSModel> HomepageModels { get; set; }
        public HomepageCMSModel()
        {
            HomepageModels = new List<HomepageCMSModel>();
        }
    }
    
}
