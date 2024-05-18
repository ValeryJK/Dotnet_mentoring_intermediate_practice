using EventBookSystem.DAL.DataContext;
using EventBookSystem.DAL.Repositories;
using EventBookSystem.Data.Entities;
using EventBookSystem.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EventBookSystem.Data.Repositories
{
    public class CartRepository : RepositoryBase<Cart>, ICartRepository
    {
        public CartRepository(MainDBContext context) : base(context) { }


        public async Task<IEnumerable<Cart>> GetAllCartsAsync(bool trackChanges) =>
           await FindAll(trackChanges).ToListAsync();

        public async Task<Cart> GetCartById(Guid cartId)
        {
            return await _context.Carts.Include(x => x.CartItems)
                .ThenInclude(x => x.Seat).ThenInclude(x => x.Price).FirstAsync(x => x.UUIDKey == cartId);
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsByCartId(Guid cartId)
        {
            return await _context.CartItems
                .Include(x => x.Seat).Where(s => s.CartId == cartId).ToListAsync();
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsByPaymentId(Guid paymentId)
        {
            return await _context.CartItems
                .Include(x => x.Seat).Where(s => s.PaymentId == paymentId).ToListAsync();
        }
    }
}