using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common;

namespace smartFunds.Data.Models.Contactbase
{
    [Table("dbo.LocalityView")]
    public class Locality : IAutoCompleteModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Name { get; set; }

        public string CountryCode { get; set; }
        public ICollection<Sublocality> Sublocalities { get; set; }

        public AutoCompleteItem AutoCompleteItem
        {
            get
            {
                return new AutoCompleteItem
                {
                    SetName = string.Format(Constants.RedisAutocomplete.SetNameFormat, this.GetType().ToString()),
                    Key = string.Format("{0} ({1})", this.Name, this.CountryCode),
                    Value = this.Id
                };
            }
        }
    }
}
