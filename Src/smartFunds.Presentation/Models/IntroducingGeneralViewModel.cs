using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Models
{
    public class IntroducingGeneralViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Desktop Banner")]
        public string Banner { get; set; }
        [Display(Name = "Mobile Banner")]
        public string MobileBanner { get; set; }
        [Display(Name = "Mô tả")]
        [Required(ErrorMessageResourceName = "FieldEmpty", ErrorMessageResourceType = typeof(Model.Resources.ValidationMessages))]
        public string Description { get; set; }
        public IFormFile BannerFile { get; set; }
        public IFormFile MobileBannerFile { get; set; }
        public string IsDeleteDesktopBanner { get; set; }
        public string IsDeleteMobileBanner { get; set; }
    }
}
