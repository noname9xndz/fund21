using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using smartFunds.Presentation.Mapper;
using smartFunds.Presentation.Models;
using smartFunds.Common.Options;
using smartFunds.Service.Models;
using smartFunds.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace smartFunds.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventGuestsController : ControllerBase
    {
        private readonly IEventGuestService _eventmemberService;
        private readonly IMemberService _memberService;
        private readonly IOptions<AppSettingsOptions> _appSettingsOptions;
        public EventGuestsController(IEventGuestService eventmemberService, IMemberService memberService, IOptions<AppSettingsOptions> appSettingsOptions)
        {
            _eventmemberService = eventmemberService;
            _memberService = memberService;
            _appSettingsOptions = appSettingsOptions;
        }

        /// <summary>
        /// Get eventr members
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetEventGuest(int eventId)
        {
            var eventMember = await _eventmemberService.GetEventGuest(eventId);
            return Ok(eventMember);
        }

        /// <summary>
        /// Update eventmember
        /// </summary>
        /// <param name="roleModel"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateEventMembers(EventMemberRoleModel roleModel)
        {
            await _eventmemberService.UpdateEventMembers(new EventMemberRole
            {
                EventGuestIds = roleModel.EventGuestIds,
                HouseholderIds = roleModel.HouseholderIds,
                EventId = roleModel.EventId,
                IsHost = roleModel.IsHost,
                IsAway = roleModel.IsAway,
                IsTba = roleModel.IsTba
            });

            return Ok();
        }

        /// <summary>
        /// Update eventmember
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddEventMembers([FromBody]List<EventGuestModel> model)
        {
            if (model == null || !model.Any()) return BadRequest();

            await _eventmemberService.AddEventMembers(model);
            return Ok();
        }

        /// <summary>
        /// Update eventmember
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("addhosts")]
        public async Task<IActionResult> AddEventHosts(MemberHostModel model)
        {
            await _eventmemberService.AddEventHosts(new MemberHost
            {
                HouseholderIds = model.HouseholderIds,
                EventId = model.EventId
            });
            return Ok();
        }

        /// <summary>
        /// Get eventr members
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("mealstatistics")]
        public async Task<IActionResult> GetMealStatistics(int eventId)
        {
            var mealStatistics = await _eventmemberService.GetMealStatistics(eventId);
            return Ok(mealStatistics);
        }

        /// <summary>
        /// Get all away member in event
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("awaymembers")]
        public async Task<IActionResult> GetAwayMembers(int eventId)
        {
            var awayMembers = await _eventmemberService.GetAwayMemberList(eventId);
            return Ok(awayMembers);
        }

        /// <summary>
        /// Search member to be assign as guest
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("searchmember")]
        public async Task<IActionResult> SearchMemberAsGuest(GuestSearchModel model)
        {
            var result = await _memberService.SearchMemberForAssignEventGuest(new GuestSearch
            {
                SearchPhase = model.SearchPhase,
                EventId = model.EventId,
                CountryCodes = model.CountryCodes,
                LocalityIds = model.LocalityIds,
                SublocalityIds = model.SublocalityIds
            });

            return Ok(result.ToMemberResult(_appSettingsOptions.Value.ContactBaseCloudfrontDistributionDomain));
        }

        /// <summary>
        /// Search member to be assign as guest
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("searchhousehold")]
        public async Task<IActionResult> SearchHouseholdAsHost(HostSearchModel model)
        {
            var result = await _memberService.SearchHostForAssignEventHost(new HostSearch
            {
                SearchPhase = model.SearchPhase,
                EventId = model.EventId,
                LocalityId = model.LocalityId,
                SublocalityIds = model.SublocalityIds
            });

            return Ok(result);
        }

        /// <summary>
        /// Delete event members
        /// </summary>
        /// <param name="eventMemberIds"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteEventMembers([FromBody] List<int> eventMemberIds)
        {
            await _eventmemberService.DeleteEventMember(eventMemberIds);

            return Ok();
        }
    }
}