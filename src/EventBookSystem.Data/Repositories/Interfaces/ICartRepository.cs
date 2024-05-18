using EventBookSystem.Data.Entities;

namespace EventBookSystem.Data.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAllCartsAsync(bool trackChanges);

        Task<Cart> GetCartById(Guid cartId);

        Task<IEnumerable<CartItem>> GetCartItemsByCartId(Guid cartId);

        Task<IEnumerable<CartItem>> GetCartItemsByPaymentId(Guid paymentId);

        void Create(Cart cart);

        void Update(Cart cart);
    }
}