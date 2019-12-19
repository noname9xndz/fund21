using System.Collections.Generic;
using smartFunds.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using smartFunds.Presentation.Models;
using smartFunds.Service.Models;

namespace smartFunds.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MealAllocationsController : ControllerBase
    {
        private readonly IMealAllocationService _mealAllocationService;
        public MealAllocationsController(IMealAllocationService mealAllocationService)
        {
            _mealAllocationService = mealAllocationService;
        }

        /// <summary>
        /// Get meal host allocation by event id 
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("mealhosts")]
        public async Task<IActionResult> GetMealHostAllocation(int eventId)
        {
            var result = await _mealAllocationService.GetMealHostAllocation(eventId);
            return Ok(result);
        }

        /// <summary>
        /// Get meal allocation statistic by event id 
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("mealallocationstatistic")]
        public async Task<IActionResult> GetMealAllocationStatistic(int eventId)
        {
            var result = await _mealAllocationService.GetMealAllocationStatistics(eventId);
            return Ok(result);
        }

        /// <summary>
        /// Get all household allocated into host
        /// </summary>
        /// <param name="eventHostId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("householdallocated")]
        public async Task<IActionResult> GetHouseholdAllocatedInHost(int eventHostId)
        {
            var result = await _mealAllocationService.GetHouseholdAllocatedInHost(eventHostId);
            return Ok(result);
        }

        /// <summary>
        /// Get all pass meal together of guest household and host
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("passmealtogether")]
        public async Task<IActionResult> GetPassMealWithCurrentHost(PassMealSearchModel model)
        {
            var result = await _mealAllocationService.GetPassMealWithCurrentHost(new PassMealSearch
            {
                EventHostId = model.EventHostId,
                MemberIds = model.MemberIds
            });

            return Ok(result);
        }

        /// <summary>
        /// Cancel meal allocations
        /// </summary>
        /// <param name="eventHostIds"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> CancelAllocation([FromForm] List<int> eventHostIds)
        {
            await _mealAllocationService.CancelAllocation(eventHostIds);

            return Ok();
        }
    }
}
