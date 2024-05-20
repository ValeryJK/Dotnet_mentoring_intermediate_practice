using EventBookSystem.DAL.Entities;
using EventBookSystem.Data.Entities;

namespace EventBookSystem.DAL.Repositories.Interfaces
{
    public interface IEventRepository : IRepositoryBase<Event>
    {
        Task<IEnumerable<Event>> GetAllEventsAsync(bool trackChanges);

        Task<IEnumerable<Seat>> GetSeatsBySection(Guid eventId, Guid sectionId, bool trackChanges);

        Task<Event?> GetEventByIdAsync(Guid eventId, bool trackChanges);       
    }
}