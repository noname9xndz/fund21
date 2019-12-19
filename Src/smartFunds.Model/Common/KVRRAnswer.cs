using System.ComponentModel.DataAnnotations;

namespace smartFunds.Model.Common
{
    public class KVRRAnswer
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public int Mark { get; set; }
    }
}
