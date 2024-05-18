using EventBookSystem.Common.DTO;

namespace EventBookSystem.Core.Service.Services.Interfaces
{
    public interface IVenueService
    {
        Task<IEnumerable<VenueDTO>> GetAllVenuesAsync(bool trackChanges = false);

        Task<IEnumerable<SectionDto>> GetSectionsByVenueAsync(Guid venueId, bool trackChanges = false);

        Task<VenueDTO?> GetVenueByIdAsync(Guid venueId, bool trackChanges = false);

        Task<VenueDTO> CreateVenueAsync(VenueForCreationDto venueDto);

        Task UpdateVenueAsync(Guid venueId, VenueForUpdateDto venueDto, bool trackChanges = false);

        Task DeleteVenueAsync(Guid venueId, bool trackChanges = false);
    }
}