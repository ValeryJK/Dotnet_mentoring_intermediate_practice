using EventBookSystem.Common.DTO;
using EventBookSystem.Common.Models;
using System.Threading;

namespace EventBookSystem.Core.Service.Services.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartItemDto>> GetCartItemsByCartId(Guid cartId, bool trackChanges = false);

        Task<CartDto?> AddSeatToCartAsync(Guid cartId, SeatRequest payload);

        Task<Guid?> BookCartPessimisticConcurrencyAsync(Guid cartId, CancellationToken cancellationToken = default);

        Task<Guid?> BookCartOptimisticConcurrencyAsync(Guid cartId);

        Task<bool> DeleteSeatFromCartAsync(Guid cartId, Guid eventId, Guid seatId);
    }
}