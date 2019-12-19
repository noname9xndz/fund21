using System.Collections.Generic;

namespace smartFunds.Service.Models
{
    public class Sublocality
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LocalityId { get; set; }     
        public Locality Locality { get; set; }
    }

    public class SublocalitySearch
    {
        public List<int> LocalityIds { get; set; }
    }
}