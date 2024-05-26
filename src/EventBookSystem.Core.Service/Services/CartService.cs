using AutoMapper;
using EventBookSystem.Common.DTO;
using EventBookSystem.Common.Models;
using EventBookSystem.Core.Service.Services.Interfaces;
using EventBookSystem.Data.Entities;
using EventBookSystem.Data.Enums;
using EventBookSystem.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using PaymentStatus = EventBookSystem.Data.Enums.PaymentStatus;

namespace EventBookSystem.Core.Service.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<CartService> _logger;       
        private readonly IMapper _mapper;
        private readonly ILockManager _lockManager;

        public CartService(ICartRepository cartRepository, ICartItemRepository cartItemRepository, IPaymentRepository paymentRepository,
            ILogger<CartService> logger, IMapper mapper, ILockManager lockManager)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _paymentRepository = paymentRepository;           
            _logger = logger;
            _mapper = mapper;
            _lockManager = lockManager;
            
        }

        public async Task<IEnumerable<CartItemDto>> GetCartItemsByCartId(Guid cartId, bool trackChanges = default)
        {
            var cartItems = await _cartRepository.GetCartItemsByCartId(cartId);

            return _mapper.Map<IEnumerable<CartItemDto>>(cartItems);
        }

        public async Task<CartDto?> AddSeatToCartAsync(Guid cartId, SeatRequest payload)
        {
            var cart = await _cartRepository.GetCartById(cartId);

            if (cart is null || cart.CartItems.Any(x => x.SeatId == payload.SeatId
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

            _logger.LogInformation("Cart item save successfully.");

            var updatedCart = await _cartRepository.GetCartById(cartId);

            return _mapper.Map<CartDto>(updatedCart);
        }

        public async Task<Guid?> BookCartPessimisticConcurrencyAsync(Guid cartId, CancellationToken cancellationToken = default)
        {
            using var transaction = await _cartRepository.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

            var cart = await _cartRepository.GetCartById(cartId);
            if (cart is null || !cart.CartItems.Any())
            {
                return default;
            }
            
            try
            {
                foreach (var item in cart.CartItems)
                {
                    if (item.Seat.Status == SeatStatus.Booked || item.Seat.Status == SeatStatus.Sold)
                    {
                        _logger.LogWarning("Seat {SeatId} already booked or sold.", item.SeatId);
                        throw new InvalidOperationException("Error: Seat already booked or sold.");
                    }

                    item.Seat.Status = SeatStatus.Booked;
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
                await _paymentRepository.SaveAsync();

                foreach (var item in cart.CartItems)
                {
                    item.PaymentId = payment.Id;
                }

                await transaction.CommitAsync();
                return payment.Id;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<Guid?> BookCartPessimisticLockConcurrencyAsync(Guid cartId, CancellationToken cancellationToken = default)
        {
            var cart = await _cartRepository.GetCartById(cartId);
            if (cart == null || !cart.CartItems.Any())
            {
                return default;
            }

            using var lockCart = await _lockManager.AcquireLockAsync(cart.Id);

            try
            {               
                foreach (var item in cart.CartItems)
                {
                    using (await _lockManager.AcquireLockAsync(item.SeatId))
                    {
                        if (item.Seat.Status == SeatStatus.Booked || item.Seat.Status == SeatStatus.Sold)
                        {
                            throw new InvalidOperationException("Error: Seat already booked or sold.");
                        }

                        item.Seat.Status = SeatStatus.Booked;
                    }
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
                await _paymentRepository.SaveAsync();

                foreach (var item in cart.CartItems)
                {
                    item.PaymentId = payment.Id;
                }

                return payment.Id;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Booking failed due to seat status conflict.");
                return default(Guid?);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while booking the cart.");
                return default(Guid?);
            }
        }

        public async Task<Guid?> BookCartOptimisticConcurrencyAsync(Guid cartId)
        {
            var cart = await _cartRepository.GetCartById(cartId);

            if (cart is null || !cart.CartItems.Any())
            {
                return default;
            }

            try
            {
                var payment = new Payment
                {
                    Id = Guid.NewGuid(),
                    Amount = cart.CartItems.Sum(x => x.Seat.Price.Amount),
                    PaymentMethod = "VISA",
                    Status = PaymentStatus.Unpaid,
                    DateUTC = DateTime.UtcNow,
                };

                _paymentRepository.Create(payment);

                foreach (var item in cart.CartItems)
                {
                    if (item.Seat.Status == SeatStatus.Booked || item.Seat.Status == SeatStatus.Sold)
                    {
                        throw new InvalidOperationException("Error...");
                    }

                    item.Seat.Status = SeatStatus.Booked;
                    item.PaymentId = payment.Id;
                }

                await _cartRepository.SaveAsync();

                return payment.Id;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new InvalidOperationException("Error ...", ex);
            }
        }

        public async Task<bool> DeleteSeatFromCartAsync(Guid cartId, Guid eventId, Guid seatId)
        {
            var cartItems = await _cartRepository.GetCartItemsByCartId(cartId);
            var cartItem = cartItems.FirstOrDefault(x => x.CartId == cartId
                && x.SeatId == seatId && x.EventId == eventId);

            if (cartItem is not null)
            {
                _cartItemRepository.Delete(cartItem);
                await _cartItemRepository.SaveAsync();

                return true;
            }

            return false;
        }
    }
}