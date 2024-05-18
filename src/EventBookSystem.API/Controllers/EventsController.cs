﻿using EventBookSystem.Common.DTO;
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
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService) => _eventService = eventService;

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventService.GetAllEventsAsync();

            return Ok(events);
        }

        [HttpGet("{eventId}")]
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
        public async Task<IActionResult> UpdateEventAsync(Guid eventId, [FromBody] EventForUpdateDto eventDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }                

            try
            {
                await _eventService.UpdateEventAsync(eventId, eventDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{eventId}")]
        public async Task<IActionResult> DeleteEventAsync(Guid eventId)
        {
            try
            {
                await _eventService.DeleteEventAsync(eventId);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}