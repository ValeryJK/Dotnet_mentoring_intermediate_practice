using EventBookSystem.API.ActionFilters;
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
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private const int CacheDuration = 60;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        [ResponseCache(Duration = CacheDuration, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }

        [HttpGet("{eventId}")]
        [ResponseCache(Duration = CacheDuration, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> GetEventByIdAsync(Guid eventId)
        {
            var eventDto = await _eventService.GetEventByIdAsync(eventId);
            if (eventDto is null)
            {
                return NotFound();
            }

            return Ok(eventDto);
        }

        [HttpGet("{eventId}/sections/{sectionId}/seats")]
        [ResponseCache(Duration = CacheDuration, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> GetSeatsBySection(Guid eventId, Guid sectionId)
        {
            var seats = await _eventService.GetSeatsBySection(eventId, sectionId);
            return Ok(seats);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEventAsync([FromBody] EventForCreationDto eventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdEvent = await _eventService.CreateEventAsync(eventDto);
            return Ok(createdEvent);
        }

        [HttpPut("{eventId}")]
        [ServiceFilter(typeof(SpecialExceptionFilter))]
        public async Task<IActionResult> UpdateEventAsync(Guid eventId, [FromBody] EventForUpdateDto eventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _eventService.UpdateEventAsync(eventId, eventDto);
            return NoContent();
        }

        [HttpDelete("{eventId}")]
        [ServiceFilter(typeof(SpecialExceptionFilter))]
        public async Task<IActionResult> DeleteEventAsync(Guid eventId)
        {
            await _eventService.DeleteEventAsync(eventId);
            return NoContent();
        }
    }
}