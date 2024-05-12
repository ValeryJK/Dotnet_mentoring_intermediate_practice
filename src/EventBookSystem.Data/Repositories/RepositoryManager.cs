using EventBookSystem.DAL.DataContext;
using EventBookSystem.DAL.Repositories.Interfaces;
using EventBookSystem.Data.Repositories;
using EventBookSystem.Data.Repositories.Interfaces;

namespace EventBookSystem.DAL.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private MainDBContext _dbContext;

        private readonly Lazy<IEventRepository> _eventRepository;
        private readonly Lazy<IVenueRepository> _venueRepository;
        private readonly Lazy<ICartRepository> _cartRepository;
        private readonly Lazy<ICartItemRepository> _cartItemRepository;
        private readonly Lazy<IPaymentRepository> _paymentRepository;

        public RepositoryManager(MainDBContext dbContext)
        {
            _dbContext = dbContext;

            _eventRepository = new Lazy<IEventRepository>(() => new EventRepository(_dbContext));
            _venueRepository = new Lazy<IVenueRepository>(() => new VenueRepository(_dbContext));
            _cartRepository = new Lazy<ICartRepository>(() => new CartRepository(_dbContext));
            _cartItemRepository = new Lazy<ICartItemRepository>(() => new CartItemRepository(_dbContext));
            _paymentRepository = new Lazy<IPaymentRepository>(() => new PaymentRepository(_dbContext));
        }

        public IEventRepository Event => _eventRepository.Value;
        public IVenueRepository Venue => _venueRepository.Value;
        public ICartRepository Cart => _cartRepository.Value;
        public ICartItemRepository CartItem => _cartItemRepository.Value;
        public IPaymentRepository Payment => _paymentRepository.Value;

        public async Task SaveAsync() => await _dbContext.SaveChangesAsync();
    }
}
