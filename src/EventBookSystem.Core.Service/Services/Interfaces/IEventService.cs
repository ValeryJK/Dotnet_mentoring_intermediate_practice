using EventBookSystem.Common.DTO;

namespace EventBookSystem.Core.Service.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<EventDto>> GetAllEventsAsync(bool trackChanges);
        Task<IEnumerable<SeatDto>> GetSeatsBySection(Guid eventId, Guid sectionId, bool trackChanges);
        Task<EventDto?> GetEventByIdAsync(Guid eventId, bool trackChanges);
        Task<EventDto> CreateEventAsync(EventForCreationDto eventDto);
        Task UpdateEventAsync(Guid eventId, EventForUpdateDto eventDto, bool trackChanges);
        Task DeleteEventAsync(Guid eventId, bool trackChanges);
    }
}
