using EventBookSystem.DAL.Repositories.Interfaces;
using EventBookSystem.Data.Repositories.Interfaces;

namespace EventBookSystem.DAL.Repositories
{
    public interface IRepositoryManager
    {
        IEventRepository Event { get; }
        IVenueRepository Venue { get; }
        ICartRepository Cart { get; }
        ICartItemRepository CartItem { get; }
        IPaymentRepository Payment { get; }

        Task SaveAsync();
    }
}
