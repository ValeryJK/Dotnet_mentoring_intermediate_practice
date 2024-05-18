using AutoMapper;
using EventBookSystem.Common.DTO;
using EventBookSystem.Common.Models;
using EventBookSystem.Core.Service.Services.Interfaces;
using EventBookSystem.Data.Entities;
using EventBookSystem.Data.Enums;
using EventBookSystem.Data.Repositories.Interfaces;

namespace EventBookSystem.Core.Service.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, ICartItemRepository cartItemRepository, IPaymentRepository paymentRepository,
            ILoggerManager logger, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _paymentRepository = paymentRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CartItemDto>> GetCartItemsByCartId(Guid cartId, bool trackChanges)
        {
            var cartItems = await _cartRepository.GetCartItemsByCartId(cartId);

            return _mapper.Map<IEnumerable<CartItemDto>>(cartItems);
        }

        public async Task<CartDto?> AddSeatToCartAsync(Guid cartId, SeatRequest payload)
        {
            var cart = await _cartRepository.GetCartById(cartId);

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

            _cartItemRepository.Create(cartItem);
            await _cartItemRepository.SaveAsync();

            var updatedCart = await _cartRepository.GetCartById(cartId);

            return _mapper.Map<CartDto>(updatedCart);
        }

        public async Task<Guid?> BookCartAsync(Guid cartId)
        {
            var cart = await _cartRepository.GetCartById(cartId);

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

            _paymentRepository.Create(payment);

            foreach (var item in cart.CartItems)
            {
                item.Seat.Status = SeatStatus.Booked;
                item.PaymentId = payment.Id;
            }

            await _paymentRepository.SaveAsync();

            return payment.Id;
        }

        public async Task<bool> DeleteSeatFromCartAsync(Guid cartId, Guid eventId, Guid seatId)
        {
            var cartItems = await _cartRepository.GetCartItemsByCartId(cartId);
            var cartItem = cartItems.FirstOrDefault(x => x.CartId == cartId
                && x.SeatId == seatId && x.EventId == eventId);

            if (cartItem != null)
            {
                _cartItemRepository.Delete(cartItem);
                await _cartItemRepository.SaveAsync();

                return true;
            }

            return false;
        }
    }
}