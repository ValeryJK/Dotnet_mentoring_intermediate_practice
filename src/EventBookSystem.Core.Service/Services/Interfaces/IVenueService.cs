using EventBookSystem.Common.DTO;

namespace EventBookSystem.Core.Service.Services.Interfaces
{
    public interface IVenueService
    {
        Task<IEnumerable<VenueDto>> GetAllVenuesAsync(bool trackChanges = false);

        Task<IEnumerable<SectionDto>> GetSectionsByVenueAsync(Guid venueId, bool trackChanges = false);

        Task<VenueDto?> GetVenueByIdAsync(Guid venueId, bool trackChanges = false);

        Task<VenueDto> CreateVenueAsync(VenueForCreationDto venueDto);

        Task UpdateVenueAsync(Guid venueId, VenueForUpdateDto venueDto, bool trackChanges = true);

        Task DeleteVenueAsync(Guid venueId, bool trackChanges = true);
    }
}