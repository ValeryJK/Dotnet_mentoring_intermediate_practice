using EventBookSystem.Common.DTO;
using EventBookSystem.Core.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventBookSystem.API.Controllers
{
    [Route("events")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize]
    public class EventsController : Controller
    {
        private readonly IServiceManager _services;

        public EventsController(IServiceManager service) => _services = service;

        /// <summary>
        /// Get the list of all events
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _services.EventService.GetAllEventsAsync();

            return Ok(events);
        }

        /// <summary>
        /// Get event by Id
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetEventByIdAsync(Guid eventId)
        {
            var eventDto = await _services.EventService.GetEventByIdAsync(eventId, trackChanges: false);

            if (eventDto == null)
                return NotFound();

            return Ok(eventDto);
        }

        /// <summary>
        /// List of seats (section_id, row_id, seat_id) with seats’ status (id, name) and price options (id, name)
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="sectionId"></param>
        /// <returns></returns>
        [HttpGet("{eventId}/sections/{sectionId}/seats")]
        public async Task<IActionResult> GetSeatsBySection(Guid eventId, Guid sectionId)
        {
            var seats = await _services.EventService.GetSeatsBySection(eventId, sectionId, trackChanges: false);

            return Ok(seats);
        }

        /// <summary>
        /// Add event
        /// </summary>
        /// <param name="eventDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateEventAsync([FromBody] EventForCreationDto eventDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdEvent = await _services.EventService.CreateEventAsync(eventDto);

            return Ok(createdEvent);
        }

        /// <summary>
        /// Update event by Id
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="eventDto"></param>
        /// <returns></returns>
        [HttpPut("{eventId}")]
        public async Task<IActionResult> UpdateEventAsync(Guid eventId, [FromBody] EventForUpdateDto eventDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _services.EventService.UpdateEventAsync(eventId, eventDto, trackChanges: true);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Delete event by Id
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpDelete("{eventId}")]
        public async Task<IActionResult> DeleteEventAsync(Guid eventId)
        {
            try
            {
                await _services.EventService.DeleteEventAsync(eventId, trackChanges: true);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}