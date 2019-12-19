using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using smartFunds.Caching.AutoComplete;
using smartFunds.Common;

namespace smartFunds.Data.Models.Contactbase
{
    [Table("dbo.MemberView")]
    public class Member : IAutoCompleteModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MaidenName { get; set; }

        public int? HouseholderId { get; set; }

        public int? FatherId { get; set; }

        public int? MotherId { get; set; }

        public int? SpouseId { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string HomePhone { get; set; }

        public string WorkPhone { get; set; }

        public string MobilePhone { get; set; }

        public string Email { get; set; }

        public bool IsHouseholder { get; set; }

        public string Household_FirstName { get; set; }

        public string Household_LastName { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string Address3 { get; set; }

        public string Address4 { get; set; }

        public string Address5 { get; set; }

        public string Address6 { get; set; }

        public string Address7 { get; set; }

        public string Address8 { get; set; }

        public string CountryCode { get; set; }

        public int? LocalityId { get; set; }

        public int? SublocalityId { get; set; }

        public string FathersName { get; set; }

        public bool? FatherDeceased { get; set; }

        public string FathersLocality { get; set; }

        public string MothersName { get; set; }

        public bool? MotherDeceased { get; set; }

        public string MothersLocality { get; set; }

        public string DeceasedSpouse { get; set; }

        public bool IsDeceased { get; set; }

        public bool IsHidden { get; set; }

        public string HomePhoneCode { get; set; }

        public string WorkPhoneCode { get; set; }

        public string MobilePhoneCode { get; set; }

        public string Title { get; set; }

        public DateTime? HomePhoneLastUpdated { get; set; }

        public DateTime? WorkPhoneLastUpdated { get; set; }

        public DateTime? MobilePhoneLastUpdated { get; set; }

        public DateTime? DeceasedDate { get; set; }

        public string FullName { get; set; }
        public string FullNameReverse { get; set; }
        public string HouseholdName { get; set; }
        public string HouseholderName { get; set; }
        public string HouseholdNameDisplay { get; set; }
        public string HouseholdCoupleName { get; set; }
        public string HouseholdLastName { get; set; }
        public int? Age { get; set; }
        [ForeignKey("LocalityId")]
        public virtual Locality Locality { get; set; }

        [ForeignKey("CountryCode")]
        public virtual Country Country { get; set; }

        [ForeignKey("SublocalityId")]
        public virtual Sublocality Sublocality { get; set; }
        public string SublocalityName { get; set; }
        public string SublocalityShortName { get; set; }
        public bool? SublocalityIsMainHall { get; set; }
        public string LocalityName { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
        public int? RegionId { get; set; }
        public string PhotoTagCdnPath { get; set; }
        public string AddressFormat { get; set; }
        public string FirstName_SC { get; set; }
        public string LastName_SC { get; set; }
        public string Generation { get; set; }
        public AutoCompleteItem AutoCompleteItem
        {
            get
            {
                var email = !string.IsNullOrEmpty(this.Email) ? " [" + this.Email + "]" : "";
                var address1 = !string.IsNullOrEmpty(this.LocalityName) ? this.LocalityName + ", " : "";
                var address2 = !string.IsNullOrEmpty(this.CountryName) ? address1 + this.CountryName : "";
                var address = address2.Length > 1 ? " (" + address2 + ")" : "";
                return new AutoCompleteItem
                {
                    SetName = string.Format(Constants.RedisAutocomplete.SetNameFormat, this.GetType().ToString()),
                    Key = string.Format("{0} {1}{2}", this.FirstName, this.LastName, address),
                    Value = this.Id
                };
            }
        }
    }
}
