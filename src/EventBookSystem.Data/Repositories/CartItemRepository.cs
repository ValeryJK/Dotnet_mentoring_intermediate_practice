using EventBookSystem.DAL.DataContext;
using EventBookSystem.DAL.Repositories;
using EventBookSystem.Data.Entities;
using EventBookSystem.Data.Repositories.Interfaces;

namespace EventBookSystem.Data.Repositories
{
    public class CartItemRepository : RepositoryBase<CartItem>, ICartItemRepository
    {
        public CartItemRepository(MainDBContext context) : base(context) { }

    }
}
