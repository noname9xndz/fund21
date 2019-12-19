using System.Collections.Generic;

namespace smartFunds.Service.Models
{
    public class Locality
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }      
        public Country Country { get; set; }
        public List<Sublocality> Sublocalities { get; set; }
    }

    public class LocalitySearch
    {
        public List<string> CountryCodes { get; set; }
    }
}