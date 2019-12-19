using smartFunds.Common.Data.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smartFunds.Data.Models
{
    /*
     * Creator by PhuongNC
     */
    public class CustomerLevel : IPersistentEntity, ITrackedEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IDCustomerLevel { get; set; }

        public string NameCustomerLevel { get; set; }

        public decimal MinMoney { get; set; }

        public decimal MaxMoney { get; set; }

        //-------
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedAt { get; set; }
        public decimal CertificateValue { get; set; }
        public decimal NAV { get; set; }
    }
}
