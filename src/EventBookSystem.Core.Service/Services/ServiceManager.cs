using AutoMapper;
using EventBookSystem.Core.Service.Services.Interfaces;
using EventBookSystem.DAL.Repositories;
using EventBookSystem.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace EventBookSystem.Core.Service.Services
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IEventService> _eventService;
        private readonly Lazy<IVenueService> _venueService;
        private readonly Lazy<ICartService> _cartService;
        private readonly Lazy<IPaymentService> _paymentService;
        private readonly Lazy<IAuthenticationService> _authenticationService;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerManager logger, UserManager<User> userManager,
            IConfiguration configuration, IMapper mapper)
        {
            _eventService = new Lazy<IEventService>(() => new EventService(repositoryManager, logger, mapper));
            _venueService = new Lazy<IVenueService>(() => new VenueService(repositoryManager, logger, mapper));
            _cartService = new Lazy<ICartService>(() => new CartService(repositoryManager, logger, mapper));
            _paymentService = new Lazy<IPaymentService>(() => new PaymentService(repositoryManager, logger, mapper));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(logger, mapper, userManager, configuration));
        }

        public IEventService EventService => _eventService.Value;

        public IVenueService VenueService => _venueService.Value;

        public ICartService CartService => _cartService.Value;

        public IPaymentService PaymentService => _paymentService.Value;

        public IAuthenticationService AuthenticationService => _authenticationService.Value;
    }
}