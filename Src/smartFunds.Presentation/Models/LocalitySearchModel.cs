using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace smartFunds.Presentation.Models
{
    public class LocalitySearchModel
    {
        [Required]
        public List<string> CountryCodes { get; set; }
    }
}
