using smartFunds.Caching.AutoComplete;
using smartFunds.Common;
using System.ComponentModel.DataAnnotations;

namespace smartFunds.Data.Models
{
    public class Setting : IAutoCompleteModel
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
        public AutoCompleteItem AutoCompleteItem
        {
            get
            {
                return new AutoCompleteItem
                {
                    SetName = string.Format(Constants.RedisAutocomplete.SetNameFormat, GetType().ToString()),
                    Key = string.Format("{0}", Key),
                    Value = Key
                };
            }
        }
    }
}
