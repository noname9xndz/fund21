using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartFunds.Model.Common
{
    public class KVRRAnswer
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }

        [Display(Name = "Mark", ResourceType = typeof(Resources.Common))]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value greater than {1}")]
        public int? Mark { get; set; }
    }
}
