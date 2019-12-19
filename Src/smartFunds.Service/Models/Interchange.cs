using System;
using System.Collections.Generic;
using System.Text;

namespace smartFunds.Service.Models
{
    public class Interchange
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public int MainLocalityId { get; set; }
        public string MainLocalityName { get; set; }
        public string EmailAddress { get; set; }
        public List<Locality> Localities { get; set; }        
    }

    public class MemberInterchangeData
    {
        public int MemberId { get; set; }
        public int LocalityId { get; set; }
        public string CountryCode { get; set; }
        public int RegionId { get; set; }
        public bool IsCoodinatorRole { get; set; }
        public List<Country> Countries { get; set; }     
    }
}
