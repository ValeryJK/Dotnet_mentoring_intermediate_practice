using EventBookSystem.Common.DTO;
using EventBookSystem.Core.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventBookSystem.API.Controllers
{
    [Route("venues")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize]
    public class VenuesController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenuesController(IVenueService venueService) => _venueService = venueService;

        [HttpGet]
        public async Task<IActionResult> GetAllVenues()
        {
            var venues = await _venueService.GetAllVenuesAsync();

            return Ok(venues);
        }

        [HttpGet("{venueId}")]
        public async Task<IActionResult> GetVenueByIdAsync(Guid venueId)
        {
            var venueDto = await _venueService.GetVenueByIdAsync(venueId);

            if (venueDto is null)
            {
                return NotFound();
            }

            return Ok(venueDto);
        }

        [HttpGet("{venueId}/sections")]
        public async Task<IActionResult> GetSectionsByVenueAsync(Guid venueId)
        {
            var sections = await _venueService.GetSectionsByVenueAsync(venueId);

            return Ok(sections);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVenueAsync([FromBody] VenueForCreationDto venueDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }                

            var createdVenue = await _venueService.CreateVenueAsync(venueDto);

            return Ok(createdVenue);
        }

        [HttpPut("{venueId}")]
        public async Task<IActionResult> UpdateVenueAsync(Guid venueId, [FromBody] VenueForUpdateDto venueDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _venueService.UpdateVenueAsync(venueId, venueDto);

            return NoContent();
        }

        [HttpDelete("{venueId}")]
        public async Task<IActionResult> DeleteEventAsync(Guid venueId)
        {
            await _venueService.DeleteVenueAsync(venueId);

            return NoContent();
        }
    }
}