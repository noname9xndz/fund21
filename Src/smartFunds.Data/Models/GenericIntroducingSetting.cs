using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace smartFunds.Data.Models
{
    public class GenericIntroducingSetting
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Banner { get; set; }
        public string MobileBanner { get; set; }
        public string Description { get; set; }
        [NotMapped]
        public IFormFile BannerFile { get; set; }
        [NotMapped]
        public IFormFile MobileBannerFile { get; set; }
    }
}
