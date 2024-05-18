using EventBookSystem.DAL.Entities;
using EventBookSystem.Data.Entities;

namespace EventBookSystem.DAL.Repositories.Interfaces
{
    public interface IVenueRepository
    {
        Task<IEnumerable<Venue>> GetAllVenuesAsync(bool trackChanges);

        Task<IEnumerable<Section>> GetSectionsByVenueAsync(Guid venueId, bool trackChanges);

        Task<Venue?> GetVenueByIdAsync(Guid venueId, bool trackChanges);

        void UpdateVenue(Venue eventEntity);

        void CreateVenue(Venue eventEntity);

        void DeleteVenue(Venue eventEntity);
    }
}