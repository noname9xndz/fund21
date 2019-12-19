using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace smartFunds.Model.Common
{
    public class IntroducingProfileSettingModel
    {
        public int Id { get; set; }
        public string ImageProfile { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
    }
}
