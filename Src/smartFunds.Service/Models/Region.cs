using System.Collections.Generic;

namespace smartFunds.Service.Models
{
    public class Region
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Country> Countries { get; set; }
    }
}