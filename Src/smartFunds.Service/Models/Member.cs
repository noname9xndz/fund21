using System;

namespace smartFunds.Service.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string Title { get; set; }
      
        public string FirstName { get; set; }
    
        public string LastName { get; set; }
        public string FirstName_SC { get; set; }
        public string LastName_SC { get; set; }
        public bool IsHouseholder { get; set; }     
        public DateTime? DateOfBirth { get; set; }
        public string CountryCode { get; set; }
        public Country Country { get; set; }
        public int? LocalityId { get; set; }
        public Locality Locality { get; set; }
        public int? SublocalityId { get; set; }
        public string SublocalityName { get; set; }
        //public Sublocality Sublocality { get; set; }
        public string Email { get; set; }       
        public string Mobile { get; set; }     
        public string HomePhone { get; set; }      
        public string WorkPhone { get; set; }
        public string Address { get; set; }
        public string Household_FirstName { get; set; }
        public string Household_LastName { get; set; }
        public int? HouseholderId { get; set; }
        public int? SpouseId { get; set; }
        public bool IsMarried { get { return SpouseId.HasValue; } }       
        public string MarriedStatus { get { return IsMarried ? "Married" : "Single"; } }
        public string HouseholdName { get; set; }
        public string HouseholderName { get; set; }
        public string HouseholdNameDisplay { get; set; }
        public bool IsSpouseInvited { get; set; }
        public int HouseholdAge { get; set; }
        public string HouseholdCoupleName { get; set; }
        public string HouseholdLastName { get; set; }
        public int? Age { get; set; }
        public string PhotoPath { get; set; }
        public string MemberPhotoUrl { get; set; }
        public string MobilePhoneCode { get; set; }
        public string HomePhoneCode { get; set; }
        public string FormattedAddress { get; set; }
        public string LocalityName { get; set; }
        public string CountryName { get; set; }
        public string FullName { get; set; }
        public string RegionName { get; set; }
        public int? RegionId { get; set; }
        public string MiddleName { get; set; }
    }
}