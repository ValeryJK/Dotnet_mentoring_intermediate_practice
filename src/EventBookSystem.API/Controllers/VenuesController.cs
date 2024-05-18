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
    public class VenuesController : Controller
    {
        private readonly IServiceManager _services;

        public VenuesController(IServiceManager service) => _services = service;

        [HttpGet]
        public async Task<IActionResult> GetAllVenues()
        {
            var venues = await _services.VenueService.GetAllVenuesAsync();

            return Ok(venues);
        }

        [HttpGet("{venueId}")]
        public async Task<IActionResult> GetVenueByIdAsync(Guid venueId)
        {
            var venueDto = await _services.VenueService.GetVenueByIdAsync(venueId);

            if (venueDto == null)
                return NotFound();

            return Ok(venueDto);
        }

        [HttpGet("{venueId}/sections")]
        public async Task<IActionResult> GetSectionsByVenueAsync(Guid venueId)
        {
            var sections = await _services.VenueService.GetSectionsByVenueAsync(venueId);

            return Ok(sections);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVenueAsync([FromBody] VenueForCreationDto venueDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdVenue = await _services.VenueService.CreateVenueAsync(venueDto);

            return Ok(createdVenue);
        }

        [HttpPut("{venueId}")]
        public async Task<IActionResult> UpdateVenueAsync(Guid venueId, [FromBody] VenueForUpdateDto venueDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _services.VenueService.UpdateVenueAsync(venueId, venueDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{venueId}")]
        public async Task<IActionResult> DeleteEventAsync(Guid venueId)
        {
            try
            {
                await _services.VenueService.DeleteVenueAsync(venueId);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}