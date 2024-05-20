using AutoMapper;
using EventBookSystem.Common.DTO;
using EventBookSystem.Core.Service.Services.Interfaces;
using EventBookSystem.DAL.Entities;
using EventBookSystem.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace EventBookSystem.Core.Service.Services
{
    public sealed class VenueService : IVenueService
    {
        private readonly IVenueRepository _venueRepository;
        private readonly ILogger<VenueService> _logger;
        private readonly IMapper _mapper;

        public VenueService(IVenueRepository venueRepository, ILogger<VenueService> logger, IMapper mapper)
        {
            _venueRepository = venueRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VenueDto>> GetAllVenuesAsync(bool trackChanges = default)
        {
            var venues = await _venueRepository.GetAllVenuesAsync(trackChanges);
            var venuesDto = _mapper.Map<IEnumerable<VenueDto>>(venues);

            return venuesDto;
        }

        public async Task<IEnumerable<SectionDto>> GetSectionsByVenueAsync(Guid venueId, bool trackChanges = default)
        {
            var sections = await _venueRepository.GetSectionsByVenueAsync(venueId, trackChanges);
            var sectionsDto = _mapper.Map<IEnumerable<SectionDto>>(sections);

            return sectionsDto;
        }

        public async Task<VenueDto?> GetVenueByIdAsync(Guid venueId, bool trackChanges = default)
        {
            var venueEntity = await _venueRepository.GetVenueByIdAsync(venueId, trackChanges);

            return venueEntity != null ? _mapper.Map<VenueDto>(venueEntity) : null;
        }

        public async Task<VenueDto> CreateVenueAsync(VenueForCreationDto venueDto)
        {
            var venueEntity = _mapper.Map<Venue>(venueDto);
            _venueRepository.Create(venueEntity);

            await _venueRepository.SaveAsync();

            return _mapper.Map<VenueDto>(venueEntity);
        }

        public async Task DeleteVenueAsync(Guid venueId, bool trackChanges = true)
        {
            var venueEntity = await _venueRepository.GetVenueByIdAsync(venueId, trackChanges);

            if (venueEntity is null)
            {
                _logger.LogError("Venue not found.");

                throw new KeyNotFoundException("Venue not found.");
            }

            _venueRepository.Delete(venueEntity);

            await _venueRepository.SaveAsync();
        }

        public async Task UpdateVenueAsync(Guid venueId, VenueForUpdateDto venueDto, bool trackChanges = true)
        {
            var venueEntity = await _venueRepository.GetVenueByIdAsync(venueId, trackChanges);

            if (venueEntity is null)
            {
                _logger.LogError("Venue not found.");

                throw new KeyNotFoundException("Venue not found.");
            }

            _mapper.Map(venueDto, venueEntity);

            await _venueRepository.SaveAsync();
        }
    }
}