using AutoMapper;
using EventBookSystem.Common.DTO;
using EventBookSystem.Common.Models;
using EventBookSystem.Core.Service.Services.Interfaces;
using EventBookSystem.DAL.Repositories;
using EventBookSystem.Data.Entities;
using EventBookSystem.Data.Enums;

namespace EventBookSystem.Core.Service.Services
{
    public class CartService : ICartService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CartService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartItemDto>> GetCartItemsByCartId(Guid cartId, bool trackChanges)
        {
            var cartItems = await _repository.Cart.GetCartItemsByCartId(cartId);

            return _mapper.Map<IEnumerable<CartItemDto>>(cartItems);
        }

        public async Task<CartDto?> AddSeatToCartAsync(Guid cartId, SeatRequest payload)
        {
            var cart = await _repository.Cart.GetCartById(cartId);

            if (cart == null || cart.CartItems.Any(x => x.SeatId == payload.SeatId
                && x.EventId == payload.EventId))
            {
                return default;
            }

            var cartItem = new CartItem
            {
                CartId = cart.Id,
                EventId = payload.EventId,
                SeatId = payload.SeatId,
                Description = "Ticket",
                DateUTC = DateTime.UtcNow,
            };

            _repository.CartItem.Create(cartItem);
            await _repository.SaveAsync();

            var updatedCart = await _repository.Cart.GetCartById(cartId);

            return _mapper.Map<CartDto>(updatedCart);
        }

        public async Task<Guid?> BookCartAsync(Guid cartId)
        {
            var cart = await _repository.Cart.GetCartById(cartId);

            if (cart == null || !cart.CartItems.Any())
            {
                return default;
            }

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                Amount = cart.CartItems.Sum(x => x.Seat.Price.Amount),
                PaymentMethod = "VISA",
                Status = PaymentStatus.Unpaid,
                DateUTC = DateTime.UtcNow
            };

            _repository.Payment.Create(payment);

            foreach (var item in cart.CartItems)
            {
                item.Seat.Status = SeatStatus.Booked;
                item.PaymentId = payment.Id;
            }

            await _repository.SaveAsync();

            return payment.Id;
        }

        public async Task<bool> DeleteSeatFromCartAsync(Guid cartId, Guid eventId, Guid seatId)
        {
            var cartItems = await _repository.Cart.GetCartItemsByCartId(cartId);
            var cartItem = cartItems.FirstOrDefault(x => x.CartId == cartId
                && x.SeatId == seatId && x.EventId == eventId);

            if (cartItem != null)
            {
                _repository.CartItem.Delete(cartItem);
                await _repository.SaveAsync();

                return true;
            }

            return false;
        }
    }
}