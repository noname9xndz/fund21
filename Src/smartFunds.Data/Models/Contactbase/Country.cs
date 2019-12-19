using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common;

namespace smartFunds.Data.Models.Contactbase
{
    [Table("dbo.CountryView")]
    public class Country : IAutoCompleteModel
    {
        [Key]
        public string Code { get; set; }

        public string Name { get; set; }

        public int RegionId { get; set; }

        public string DialingPrefix { get; set; }

        public string AddressFormat { get; set; }

        public string Address1Label { get; set; }

        public bool Address1Visible { get; set; }

        public bool Address1Required { get; set; }

        public string Address2Label { get; set; }

        public bool Address2Visible { get; set; }

        public bool Address2Required { get; set; }

        public string Address3Label { get; set; }

        public bool Address3Visible { get; set; }

        public bool Address3Required { get; set; }

        public string Address4Label { get; set; }

        public bool Address4Visible { get; set; }

        public bool Address4Required { get; set; }

        public string Address5Label { get; set; }

        public bool Address5Visible { get; set; }

        public bool Address5Required { get; set; }

        public string Address6Label { get; set; }

        public bool Address6Visible { get; set; }

        public bool Address6Required { get; set; }

        public string Address7Label { get; set; }

        public bool Address7Visible { get; set; }

        public bool Address7Required { get; set; }

        public string Address8Label { get; set; }

        public bool Address8Visible { get; set; }

        public bool Address8Required { get; set; }

        [ForeignKey("RegionId")]
        public Region Region { get; set; }
        public ICollection<Locality> Localities { get; set; }
        public AutoCompleteItem AutoCompleteItem
        {
            get
            {
                return new AutoCompleteItem
                {
                    SetName = string.Format(Constants.RedisAutocomplete.SetNameFormat, this.GetType().ToString()),
                    Key = string.Format("{0} ({1})", this.Code, this.Name),
                    Value = this.Code
                };
            }
        }
    }
}
