using AutoMapper;
using EventBookSystem.Common.DTO;
using EventBookSystem.Core.Service.Services.Interfaces;
using EventBookSystem.DAL.Entities;
using EventBookSystem.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace EventBookSystem.Core.Service.Services
{
    public sealed class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<EventService> _logger;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, ILogger<EventService> logger, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventDto>> GetAllEventsAsync(bool trackChanges)
        {
            var events = await _eventRepository.GetAllEventsAsync(trackChanges);

            return _mapper.Map<IEnumerable<EventDto>>(events);
        }

        public async Task<EventDto?> GetEventByIdAsync(Guid eventId, bool trackChanges)
        {
            var eventEntity = await _eventRepository.GetEventByIdAsync(eventId, trackChanges);

            return eventEntity != null ? _mapper.Map<EventDto>(eventEntity) : null;
        }

        public async Task<IEnumerable<SeatDto>> GetSeatsBySection(Guid eventId, Guid sectionId, bool trackChanges)
        {
            var seats = await _eventRepository.GetSeatsBySection(eventId, sectionId, trackChanges);

            return _mapper.Map<IEnumerable<SeatDto>>(seats);
        }

        public async Task<EventDto> CreateEventAsync(EventForCreationDto eventDto)
        {
            var eventEntity = _mapper.Map<Event>(eventDto);
            _eventRepository.Create(eventEntity);

            await _eventRepository.SaveAsync();

            return _mapper.Map<EventDto>(eventEntity);
        }

        public async Task UpdateEventAsync(Guid eventId, EventForUpdateDto eventDto, bool trackChanges)
        {
            var eventEntity = await _eventRepository.GetEventByIdAsync(eventId, trackChanges);

            if (eventEntity == null)
                throw new KeyNotFoundException("Event not found.");

            _mapper.Map(eventDto, eventEntity);

            await _eventRepository.SaveAsync();
        }

        public async Task DeleteEventAsync(Guid eventId, bool trackChanges)
        {
            var eventEntity = await _eventRepository.GetEventByIdAsync(eventId, trackChanges);

            if (eventEntity == null)
                throw new KeyNotFoundException("Event not found.");

            _eventRepository.Delete(eventEntity);

            await _eventRepository.SaveAsync();
        }
    }
}