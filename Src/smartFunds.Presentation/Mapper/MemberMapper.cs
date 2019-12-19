using System.Linq;
using smartFunds.Presentation.Models;
using smartFunds.Service;
using smartFunds.Service.Models;
using smartFunds.Service.Services;


namespace smartFunds.Presentation.Mapper
{
    public static class MemberMapper
    {
        public static WebSearchResult<MemberResultModel> ToMemberResult(this WebSearchResult<MemberResult> item, string distributionDomain = "d23bplak01dwmt.cloudfront.net")
        {
            if (item == null)
                return null;
            
            return new WebSearchResult<MemberResultModel>
            {
                TotalCount = item.TotalCount,
                Result = item.Result.Select(x => new MemberResultModel
                {
                    MemberId = x.MemberId,
                    Title = x.Title,
                    FirtName = x.FirtName,
                    LastName = x.LastName,
                    HouseholderId = x.HouseholderId,
                    HouseholdName = x.HouseholdName,
                    Age = x.Age,
                    RegionId = x.RegionId,
                    RegionName = x.RegionName,
                    LocalityId = x.LocalityId,
                    LocalityName = x.LocalityName,
                    Role = x.Role,
                    MemberPhotoUrl = Helpers.GetCannedSignedURLContactBase(x.PhotoPath, distributionDomain),
                    CountryCode = x.CountryCode
                })

            };
        }

    }
}
