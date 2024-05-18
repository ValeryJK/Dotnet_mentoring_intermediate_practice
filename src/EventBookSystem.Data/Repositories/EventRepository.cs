using EventBookSystem.DAL.DataContext;
using EventBookSystem.DAL.Entities;
using EventBookSystem.DAL.Repositories.Interfaces;
using EventBookSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventBookSystem.DAL.Repositories
{
    public class EventRepository : RepositoryBase<Event>, IEventRepository
    {
        public EventRepository(MainDBContext context) : base(context) { }


        public async Task<IEnumerable<Event>> GetAllEventsAsync(bool trackChanges) =>
           await GetAll(trackChanges).OrderBy(c => c.Name).ToListAsync();

        public async Task<IEnumerable<Seat>> GetSeatsBySection(Guid eventId, Guid sectionId, bool trackChanges)
        {
            return await _context.Seats
                .Include(s => s.Price)
                .Where(s => s.Section.EventId == eventId && s.SectionId == sectionId)
                .ToListAsync();
        }

        public async Task<Event?> GetEventByIdAsync(Guid eventId, bool trackChanges) =>
            await GetAll(trackChanges).Where(x => x.Id == eventId).SingleOrDefaultAsync();
    }
}