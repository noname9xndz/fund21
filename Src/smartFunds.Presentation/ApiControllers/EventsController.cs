using System;
using smartFunds.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using smartFunds.Service.Models;
using System.Linq;
using System.Net;

namespace smartFunds.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Get event by localityid and datetime
        /// </summary>
        /// <param name="localityId"></param>
        /// <param name="eventDate"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetEvent(int localityId, DateTime eventDate, int year)
        {
            if (year != 0)
            {
                var events = await _eventService.GetAllEventsInYear(localityId, Convert.ToInt32(year));
                return Ok(events);
            }

            var @event = await _eventService.GetEventByLocalityIdAndDate(localityId, eventDate);
            return Ok(@event);
        }

        // POST: api/Event
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EventInputModel eventInput)
        {
            var result = await _eventService.AddOrUpdateEventAsync(new Event
            {
                MainLocalityId = eventInput.MainLocalityId,
                EventDate = eventInput.EventDate,
                CountryCode = eventInput.CountryCode,
                Sublocalities = eventInput.SublocalityIds.Select(x => new Sublocality
                {
                    Id = x
                }).ToList()
            });

            return Ok(result);
        }

        /// <summary>
        /// Delete event by event id 
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{eventId}")]
        public async Task<IActionResult> Delete(int eventId)
        {
            var result = await _eventService.DeleteEvent(eventId);
            return result ? Ok() : StatusCode((int)HttpStatusCode.InternalServerError);
        }
    }
}
