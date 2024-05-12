namespace EventBookSystem.Core.Service.Services.Interfaces
{
    public interface IServiceManager
    {
        IEventService EventService { get; }
        IVenueService VenueService { get; }
        ICartService CartService { get; }
        IPaymentService PaymentService { get; }
        IAuthenticationService AuthenticationService { get; }
    }
}
