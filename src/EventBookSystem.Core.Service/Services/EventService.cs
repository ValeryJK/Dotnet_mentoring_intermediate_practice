using AutoMapper;
using EventBookSystem.Common.DTO;
using EventBookSystem.Core.Service.Services.Interfaces;
using EventBookSystem.DAL.Entities;
using EventBookSystem.DAL.Repositories;

namespace EventBookSystem.Core.Service.Services
{
    public sealed class EventService : IEventService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public EventService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EventDto>> GetAllEventsAsync(bool trackChanges)
        {
            var events = await _repository.Event.GetAllEventsAsync(trackChanges);

            return _mapper.Map<IEnumerable<EventDto>>(events);
        }

        public async Task<EventDto?> GetEventByIdAsync(Guid eventId, bool trackChanges)
        {
            var eventEntity = await _repository.Event.GetEventByIdAsync(eventId, trackChanges);

            return eventEntity != null ? _mapper.Map<EventDto>(eventEntity) : null;
        }

        public async Task<IEnumerable<SeatDto>> GetSeatsBySection(Guid eventId, Guid sectionId, bool trackChanges)
        {
            var seats = await _repository.Event.GetSeatsBySection(eventId, sectionId, trackChanges);

            return _mapper.Map<IEnumerable<SeatDto>>(seats);
        }

        public async Task<EventDto> CreateEventAsync(EventForCreationDto eventDto)
        {
            var eventEntity = _mapper.Map<Event>(eventDto);
            _repository.Event.CreateEvent(eventEntity);

            await _repository.SaveAsync();

            return _mapper.Map<EventDto>(eventEntity);
        }

        public async Task UpdateEventAsync(Guid eventId, EventForUpdateDto eventDto, bool trackChanges)
        {
            var eventEntity = await _repository.Event.GetEventByIdAsync(eventId, trackChanges);

            if (eventEntity == null)
                throw new KeyNotFoundException("Event not found.");

            _mapper.Map(eventDto, eventEntity);

            await _repository.SaveAsync();
        }

        public async Task DeleteEventAsync(Guid eventId, bool trackChanges)
        {
            var eventEntity = await _repository.Event.GetEventByIdAsync(eventId, trackChanges);

            if (eventEntity == null)
                throw new KeyNotFoundException("Event not found.");

            _repository.Event.DeleteEvent(eventEntity);

            await _repository.SaveAsync();
        }
    }
}