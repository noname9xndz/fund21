using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace smartFunds.Model.Common
{
    public class GenericIntroducingSettingModel
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
    }
}
