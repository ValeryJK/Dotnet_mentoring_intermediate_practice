using EventBookSystem.Common.DTO;
using EventBookSystem.Common.Models;

namespace EventBookSystem.Core.Service.Services.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartItemDto>> GetCartItemsByCartId(Guid cartId, bool trackChanges = false);

        Task<CartDto?> AddSeatToCartAsync(Guid cartId, SeatRequest payload);

        Task<Guid?> BookCartAsync(Guid cartId);

        Task<bool> DeleteSeatFromCartAsync(Guid cartId, Guid eventId, Guid seatId);
    }
}