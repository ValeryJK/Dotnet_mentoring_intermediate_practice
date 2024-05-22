using EventBookSystem.API.ActionFilters;
using EventBookSystem.Common.DTO;
using EventBookSystem.Core.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace EventBookSystem.API.Controllers
{
    [Route("events")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IMemoryCache _cache;

        public EventsController(IEventService eventService, IMemoryCache cache)
        {
            _eventService = eventService;
            _cache = cache;
        }

        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> GetAllEvents()
        {
            var cacheKey = "AllEvents";
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<EventDto>? events))
            {
                events = await _eventService.GetAllEventsAsync();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, events, cacheEntryOptions);
            }

            return Ok(events);
        }

        [HttpGet("{eventId}")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)]
        public async Task<IActionResult> GetEventByIdAsync(Guid eventId)
        {
            var cacheKey = $"Event_{eventId}";
            if (!_cache.TryGetValue(cacheKey, out EventDto? eventDto))
            {
                eventDto = await _eventService.GetEventByIdAsync(eventId);
                if (eventDto == null)
                {
                    return NotFound();
                }

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5));

                _cache.Set(cacheKey, eventDto, cacheEntryOptions);
            }

            return Ok(eventDto);
        }

        [HttpGet("{eventId}/sections/{sectionId}/seats")]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client, NoStore = false)]
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

            await InvalidateEventCache();

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

            await InvalidateEventCache();
            _cache.Remove($"Event_{eventId}");

            return NoContent();
        }

        [HttpDelete("{eventId}")]
        [ServiceFilter(typeof(SpecialExceptionFilter))]
        public async Task<IActionResult> DeleteEventAsync(Guid eventId)
        {
            await _eventService.DeleteEventAsync(eventId);

            return NoContent();
        }

        private async Task InvalidateEventCache()
        {
            _cache.Remove("AllEvents");
            await Task.CompletedTask;
        }
    }
}