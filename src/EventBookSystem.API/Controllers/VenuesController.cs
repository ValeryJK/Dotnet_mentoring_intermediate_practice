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

        /// <summary>
        /// Get the list of all venues
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllVenues()
        {
            var venues = await _services.VenueService.GetAllVenuesAsync(trackChanges: false);

            return Ok(venues);
        }

        /// <summary>
        /// Get venue by Id
        /// </summary>
        /// <param name="venueId"></param>
        /// <returns></returns>
        [HttpGet("{venueId}")]
        public async Task<IActionResult> GetVenueByIdAsync(Guid venueId)
        {
            var venueDto = await _services.VenueService.GetVenueByIdAsync(venueId, trackChanges: false);

            if (venueDto == null)
                return NotFound();

            return Ok(venueDto);
        }

        /// <summary>
        /// Returns all sections for venue
        /// </summary>
        /// <param name="venueId"></param>
        /// <returns></returns>
        [HttpGet("{venueId}/sections")]
        public async Task<IActionResult> GetSectionsByVenueAsync(Guid venueId)
        {
            var sections = await _services.VenueService.GetSectionsByVenueAsync(venueId, trackChanges: false);

            return Ok(sections);
        }

        /// <summary>
        /// Add venue
        /// </summary>
        /// <param name="venueDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateVenueAsync([FromBody] VenueForCreationDto venueDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdVenue = await _services.VenueService.CreateVenueAsync(venueDto);

            return Ok(createdVenue);
        }

        /// <summary>
        /// Update venue by Id
        /// </summary>
        /// <param name="venueId"></param>
        /// <param name="venueDto"></param>
        /// <returns></returns>
        [HttpPut("{venueId}")]
        public async Task<IActionResult> UpdateVenueAsync(Guid venueId, [FromBody] VenueForUpdateDto venueDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _services.VenueService.UpdateVenueAsync(venueId, venueDto, trackChanges: true);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Delete venue by Id
        /// </summary>
        /// <param name="venueId"></param>
        /// <returns></returns>
        [HttpDelete("{venueId}")]
        public async Task<IActionResult> DeleteEventAsync(Guid venueId)
        {
            try
            {
                await _services.VenueService.DeleteVenueAsync(venueId, trackChanges: true);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}