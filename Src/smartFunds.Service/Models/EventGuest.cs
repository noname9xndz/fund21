using System;
using System.Collections.Generic;

namespace smartFunds.Service.Models
{
    public class EventGuest
    {
        public MealSetupSectionModel GuestSection { get; set; }
        public MealSetupSectionModel ToBeAssignedSection { get; set; }
    }

    public class EventHost
    {
        public MealSetupSectionModel HostSection { get; set; }
    }

    public class MealSetupSectionModel
    {
        public List<HousehosmartFundsodel> Households { get; set; }
        public List<Locality> Localities { get; set; }
    }

    public class HousehosmartFundsodel
    {
        public int HouseholderId { get; set; }
        public string HouseholdName { get; set; }
        public int CP { get; set; }
        public int SCP { get; set; }
        public int DefaultCP { get; set; }
        public int DefaultSCP { get; set; }
        public DateTime? PreviousMeal { get; set; }
        public int TotalMealInYear { get; set; }
        public int ConsecMeal { get; set; }
        public int HousehosmartFundsembers { get; set; }
        public Region Region { get; set; }
        public Locality Locality { get; set; }
        public Sublocality Sublocality { get; set; }
        public string ContactNumber { get; set; }
        public string EmailAddress { get; set; }
        public int EventHostId { get; set; }
        public List<MemberInformationModel> Members { get; set; }
    }

    public class MemberInformationModel
    {
        public int EventGuestId { get; set; }
        public int MemberId { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public bool IsHouseholder { get; set; }
        public bool IsAway { get; set; }
        public bool WarningToChange { get; set; }
        public string ContactNumber { get; set; }
        public string EmailAddress { get; set; }
    }

    public class EventMemberRole
    {
        public List<int> EventGuestIds { get; set; }
        public List<int> HouseholderIds { get; set; }
        public bool IsHost { get; set; }
        public bool IsTba { get; set; }
        public bool IsAway { get; set; }
        public int EventId { get; set; }
    }

    public class EventGuestModel
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int MemberId { get; set; }
        public int HouseholderId { get; set; }
        public bool IsAway { get; set; }
        public bool IsToBeAssigned { get; set; }
    }

    public class MemberHost
    {
        public List<int> HouseholderIds { get; set; }
        public int EventId { get; set; }
    }
}
