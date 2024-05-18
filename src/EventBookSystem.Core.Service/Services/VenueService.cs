using AutoMapper;
using EventBookSystem.Common.DTO;
using EventBookSystem.Core.Service.Services.Interfaces;
using EventBookSystem.DAL.Entities;
using EventBookSystem.DAL.Repositories;

namespace EventBookSystem.Core.Service.Services
{
    public sealed class VenueService : IVenueService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public VenueService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<VenueDTO>> GetAllVenuesAsync(bool trackChanges)
        {
            var venues = await _repository.Venue.GetAllVenuesAsync(trackChanges);
            var venuesDto = _mapper.Map<IEnumerable<VenueDTO>>(venues);

            return venuesDto;
        }

        public async Task<IEnumerable<SectionDto>> GetSectionsByVenueAsync(Guid venueId, bool trackChanges)
        {
            var sections = await _repository.Venue.GetSectionsByVenueAsync(venueId, trackChanges);
            var sectionsDto = _mapper.Map<IEnumerable<SectionDto>>(sections);

            return sectionsDto;
        }

        public async Task<VenueDTO?> GetVenueByIdAsync(Guid venueId, bool trackChanges)
        {
            var venueEntity = await _repository.Venue.GetVenueByIdAsync(venueId, trackChanges);

            return venueEntity != null ? _mapper.Map<VenueDTO>(venueEntity) : null;
        }

        public async Task<VenueDTO> CreateVenueAsync(VenueForCreationDto venueDto)
        {
            var venueEntity = _mapper.Map<Venue>(venueDto);
            _repository.Venue.CreateVenue(venueEntity);

            await _repository.SaveAsync();

            return _mapper.Map<VenueDTO>(venueEntity);
        }

        public async Task DeleteVenueAsync(Guid venueId, bool trackChanges)
        {
            var venueEntity = await _repository.Venue.GetVenueByIdAsync(venueId, trackChanges);

            if (venueEntity == null)
                throw new KeyNotFoundException("Venue not found.");

            _repository.Venue.DeleteVenue(venueEntity);

            await _repository.SaveAsync();
        }

        public async Task UpdateVenueAsync(Guid venueId, VenueForUpdateDto venueDto, bool trackChanges)
        {
            var venueEntity = await _repository.Venue.GetVenueByIdAsync(venueId, trackChanges);

            if (venueEntity == null)
                throw new KeyNotFoundException("Venue not found.");

            _mapper.Map(venueDto, venueEntity);

            await _repository.SaveAsync();
        }
    }
}