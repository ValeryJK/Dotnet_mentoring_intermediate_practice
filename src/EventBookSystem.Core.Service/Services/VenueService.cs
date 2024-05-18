using AutoMapper;
using EventBookSystem.Common.DTO;
using EventBookSystem.Core.Service.Services.Interfaces;
using EventBookSystem.DAL.Entities;
using EventBookSystem.DAL.Repositories.Interfaces;

namespace EventBookSystem.Core.Service.Services
{
    public sealed class VenueService : IVenueService
    {
        private readonly IVenueRepository _venueRepository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public VenueService(IVenueRepository venueRepository, ILoggerManager logger, IMapper mapper)
        {
            _venueRepository = venueRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VenueDTO>> GetAllVenuesAsync(bool trackChanges)
        {
            var venues = await _venueRepository.GetAllVenuesAsync(trackChanges);
            var venuesDto = _mapper.Map<IEnumerable<VenueDTO>>(venues);

            return venuesDto;
        }

        public async Task<IEnumerable<SectionDto>> GetSectionsByVenueAsync(Guid venueId, bool trackChanges)
        {
            var sections = await _venueRepository.GetSectionsByVenueAsync(venueId, trackChanges);
            var sectionsDto = _mapper.Map<IEnumerable<SectionDto>>(sections);

            return sectionsDto;
        }

        public async Task<VenueDTO?> GetVenueByIdAsync(Guid venueId, bool trackChanges)
        {
            var venueEntity = await _venueRepository.GetVenueByIdAsync(venueId, trackChanges);

            return venueEntity != null ? _mapper.Map<VenueDTO>(venueEntity) : null;
        }

        public async Task<VenueDTO> CreateVenueAsync(VenueForCreationDto venueDto)
        {
            var venueEntity = _mapper.Map<Venue>(venueDto);
            _venueRepository.Create(venueEntity);

            await _venueRepository.SaveAsync();

            return _mapper.Map<VenueDTO>(venueEntity);
        }

        public async Task DeleteVenueAsync(Guid venueId, bool trackChanges)
        {
            var venueEntity = await _venueRepository.GetVenueByIdAsync(venueId, trackChanges);

            if (venueEntity == null)
                throw new KeyNotFoundException("Venue not found.");

            _venueRepository.Delete(venueEntity);

            await _venueRepository.SaveAsync();
        }

        public async Task UpdateVenueAsync(Guid venueId, VenueForUpdateDto venueDto, bool trackChanges)
        {
            var venueEntity = await _venueRepository.GetVenueByIdAsync(venueId, trackChanges);

            if (venueEntity == null)
                throw new KeyNotFoundException("Venue not found.");

            _mapper.Map(venueDto, venueEntity);

            await _venueRepository.SaveAsync();
        }
    }
}