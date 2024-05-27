using AutoMapper;
using EventBookSystem.Common.DTO;
using EventBookSystem.Common.Settings;
using EventBookSystem.Core.Service.Services.Interfaces;
using EventBookSystem.DAL.Entities;
using EventBookSystem.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventBookSystem.Core.Service.Services
{
    public sealed class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILogger<EventService> _logger;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly IOptionsMonitor<CacheSettings> _cacheSettings;

        public EventService(IEventRepository eventRepository, ILogger<EventService> logger, IMapper mapper, IMemoryCache cache,
            IOptionsMonitor<CacheSettings> cacheSettings)
        {
            _eventRepository = eventRepository;
            _logger = logger;
            _mapper = mapper;
            _cache = cache;
            _cacheSettings = cacheSettings;
        }

        public async Task<IEnumerable<EventDto>> GetAllEventsAsync(bool trackChanges = default)
        {
            return await _cache.GetOrCreateAsync("AllEvents", async entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromMinutes(_cacheSettings.CurrentValue.SlidingExpiration));

                var events = await _eventRepository.GetAllEventsAsync(trackChanges);

                return _mapper.Map<IEnumerable<EventDto>>(events);
            }) ?? new List<EventDto>();
        }

        public async Task<EventDto?> GetEventByIdAsync(Guid eventId, bool trackChanges = default)
        {
            return await _cache.GetOrCreateAsync($"Event_{eventId}", async entry =>
            {
                entry.SetSlidingExpiration(TimeSpan.FromMinutes(_cacheSettings.CurrentValue.SlidingExpiration));

                var eventEntity = await _eventRepository.GetEventByIdAsync(eventId, trackChanges);
                return eventEntity != null ? _mapper.Map<EventDto>(eventEntity) : null;
            });
        }

        public async Task<IEnumerable<SeatDto>> GetSeatsBySection(Guid eventId, Guid sectionId, bool trackChanges = default)
        {
            var seats = await _eventRepository.GetSeatsBySection(eventId, sectionId, trackChanges);

            return _mapper.Map<IEnumerable<SeatDto>>(seats);
        }

        public async Task<EventDto> CreateEventAsync(EventForCreationDto eventDto)
        {
            var eventEntity = _mapper.Map<Event>(eventDto);
            _eventRepository.Create(eventEntity);

            await _eventRepository.SaveAsync();
            InvalidateCache();

            return _mapper.Map<EventDto>(eventEntity);
        }

        public async Task UpdateEventAsync(Guid eventId, EventForUpdateDto eventDto, bool trackChanges = true)
        {
            var eventEntity = await _eventRepository.GetEventByIdAsync(eventId, trackChanges);

            if (eventEntity is null)
            {
                _logger.LogError("Event not found.");

                throw new KeyNotFoundException("Event not found.");
            }

            _mapper.Map(eventDto, eventEntity);

            await _eventRepository.SaveAsync();
            InvalidateCache();
        }

        public async Task DeleteEventAsync(Guid eventId, bool trackChanges = true)
        {
            var eventEntity = await _eventRepository.GetEventByIdAsync(eventId, trackChanges);

            if (eventEntity is null)
            {
                _logger.LogError("Event not found.");

                throw new KeyNotFoundException("Event not found.");
            }

            _eventRepository.Delete(eventEntity);

            await _eventRepository.SaveAsync();
            InvalidateCache();
        }

        private void InvalidateCache()
        {
            _cache.Remove("AllEvents");
        }
    }
}