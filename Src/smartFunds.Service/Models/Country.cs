using System.Collections.Generic;

namespace smartFunds.Service.Models
{
    public class Country
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int RegionId { get; set; }
        public Region Region { get; set; }
        public List<Locality> Localities { get; set; }
    }
}