using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartFunds.Data.Models.Contactbase
{
    [Table("dbo.SublocalityView")]
    public class Sublocality
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }

        public int LocalityId { get; set; }

        public string ShortName { get; set; }

        public bool IsMainHall { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string Address4 { get; set; }

        public string Address5 { get; set; }

        public string Address6 { get; set; }

        public string Address7 { get; set; }

        public string Address8 { get; set; }

        [ForeignKey("LocalityId")]
        public virtual Locality Locality { get; set; }
    }
}
