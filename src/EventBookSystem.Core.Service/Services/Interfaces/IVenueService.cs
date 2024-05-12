using EventBookSystem.Common.DTO;

namespace EventBookSystem.Core.Service.Services.Interfaces
{
    public interface IVenueService
    {
        Task<IEnumerable<VenueDTO>> GetAllVenuesAsync(bool trackChanges);
        Task<IEnumerable<SectionDto>> GetSectionsByVenueAsync(Guid venueId, bool trackChanges);
        Task<VenueDTO?> GetVenueByIdAsync(Guid venueId, bool trackChanges);
        Task<VenueDTO> CreateVenueAsync(VenueForCreationDto venueDto);
        Task UpdateVenueAsync(Guid venueId, VenueForUpdateDto venueDto, bool trackChanges);
        Task DeleteVenueAsync(Guid venueId, bool trackChanges);
    }
}
