using EventBookSystem.Data.Entities;

namespace EventBookSystem.Data.Repositories.Interfaces
{
    public interface ICartItemRepository
    {
        void Create(CartItem cartItem);

        void Update(CartItem cartItem);

        void Delete(CartItem cartItem);
    }
}