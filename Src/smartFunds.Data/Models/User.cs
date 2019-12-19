using Microsoft.AspNetCore.Identity;
using smartFunds.Common.Data.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace smartFunds.Data.Models
{
    public class User : IdentityUser, IPersistentEntity, ITrackedEntity
    {
        [PersonalData]
        public string FullName { get; set; }
        [PersonalData]
        public decimal InitialInvestmentAmount { get; set; }
        [PersonalData]
        public decimal CurrentAccountAmount { get; set; }
        public DateTime Created { get; set; }
        public DateTime DateLastUpdated { get; set; }
        public string LastUpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedAt { get; set; }
    }
}
