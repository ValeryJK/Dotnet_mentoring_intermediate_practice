using EventBookSystem.DAL.DataContext;
using EventBookSystem.DAL.Entities;
using EventBookSystem.DAL.Repositories.Interfaces;
using EventBookSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventBookSystem.DAL.Repositories
{
    public class VenueRepository : RepositoryBase<Venue>, IVenueRepository
    {
        public VenueRepository(MainDBContext context) : base(context) { }

        public async Task<IEnumerable<Venue>> GetAllVenuesAsync(bool trackChanges) =>
          await FindAll(trackChanges).OrderBy(c => c.Name).ToListAsync();

        public async Task<IEnumerable<Section>> GetSectionsByVenueAsync(Guid venueId, bool trackChanges)
        {
            return await _context.Sections.Where(s => s.VenueId == venueId).ToListAsync();
        }

        public async Task<Venue?> GetVenueByIdAsync(Guid venueId, bool trackChanges) =>
           await FindAll(trackChanges).Where(x => x.Id == venueId).SingleOrDefaultAsync();

        public void UpdateVenue(Venue venueEntity) => Update(venueEntity);

        public void CreateVenue(Venue venueEntity) => Create(venueEntity);

        public void DeleteVenue(Venue venueEntity) => Delete(venueEntity);
    }
}