using smartFunds.Presentation.Models;
using smartFunds.Caching.AutoComplete;
using smartFunds.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace smartFunds.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutoCompleteController : ControllerBase
    {
        private readonly IAutoCompleteService _autoCompleteService;
        public AutoCompleteController(IAutoCompleteService autoCompleteService)
        {
            _autoCompleteService = autoCompleteService;
        }

        // POST: api/autocomplete
        [HttpPost]
        [Route("")]
       public async Task<ActionResult<AutoCompleteItem>> Build(AutoCompleteRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(await _autoCompleteService.GetItemsAsync(request.Type, request.Key));
        }
    }
}