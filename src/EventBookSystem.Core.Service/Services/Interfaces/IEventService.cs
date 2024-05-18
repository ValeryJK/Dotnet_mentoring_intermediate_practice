using EventBookSystem.Common.DTO;

namespace EventBookSystem.Core.Service.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<EventDto>> GetAllEventsAsync(bool trackChanges = false);

        Task<IEnumerable<SeatDto>> GetSeatsBySection(Guid eventId, Guid sectionId, bool trackChanges = false);

        Task<EventDto?> GetEventByIdAsync(Guid eventId, bool trackChanges = false);

        Task<EventDto> CreateEventAsync(EventForCreationDto eventDto);

        Task UpdateEventAsync(Guid eventId, EventForUpdateDto eventDto, bool trackChanges = false);

        Task DeleteEventAsync(Guid eventId, bool trackChanges = false);
    }
}