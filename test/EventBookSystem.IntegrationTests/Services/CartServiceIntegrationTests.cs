using AutoMapper;
using EventBookSystem.API;
using EventBookSystem.Common.Models;
using EventBookSystem.Core.Service.Services.Interfaces;
using EventBookSystem.DAL.DataContext;
using EventBookSystem.Data.Entities;
using EventBookSystem.Data.Enums;
using EventBookSystem.IntegrationTests.Services.Initialize;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EventBookSystem.IntegrationTests.Services
{
    public class CartServiceIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly ICartService _cartService;
        private readonly MainDBContext _context;
        private readonly IMapper _mapper;

        public CartServiceIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            var scope = factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<MainDBContext>();
            _cartService = scope.ServiceProvider.GetRequiredService<ICartService>();
            _mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetCartItemsByCartId_ShouldReturnCartItems()
        {
            // Arrange
            var cart = new Cart
                { 
                    Id = Guid.NewGuid() 
                };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            var seat = new Seat 
                { 
                    Id = Guid.NewGuid(), 
                    Row = 1, 
                    Number = 1, 
                    Price = new Price 
                        { 
                            Name = "VIP", 
                            Amount = 100 
                        }, 
                    Status = SeatStatus.Available 
                };
            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();

            var cartItem = new CartItem 
                { 
                    Id = Guid.NewGuid(), 
                    SeatId = seat.Id, 
                    EventId = Guid.NewGuid(), 
                    CartId = cart.Id, 
                    DateUTC = DateTime.UtcNow 
                };
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            // Act
            var cartItems = await _cartService.GetCartItemsByCartId(cart.Id, false);

            // Assert
            cartItems.Should().NotBeNull();
            cartItems.Should().ContainSingle();
        }

        [Fact]
        public async Task AddSeatToCart_ShouldReturnUpdatedCart()
        {
            // Arrange          
            var cart = new Cart { Id = Guid.NewGuid(), UUIDKey = Guid.NewGuid() };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            var seat = new Seat
            {
                Id = Guid.NewGuid(),
                Row = 1,
                Number = 1,
                Price = new Price
                {
                    Name = "VIP",
                    Amount = 100
                },
                Status = SeatStatus.Available
            };
            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();

            var seatRequest = new SeatRequest 
                { 
                    SeatId = seat.Id, 
                    EventId = Guid.NewGuid(), 
                    PriceId = Guid.NewGuid() 
                };

            // Act
            var updatedCart = await _cartService.AddSeatToCartAsync(cart.UUIDKey, seatRequest);

            // Assert
            updatedCart.Should().NotBeNull();
            updatedCart.CartItems.Should().ContainSingle();
        }

        [Fact]
        public async Task DeleteSeatFromCart_ShouldReturnTrue()
        {
            // Arrange
            var cart = new Cart { Id = Guid.NewGuid() };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            var seat = new Seat
            {
                Id = Guid.NewGuid(),
                Row = 1,
                Number = 1,
                Price = new Price
                {
                    Name = "VIP",
                    Amount = 100
                },
                Status = SeatStatus.Available
            };
            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();

            var cartItem = new CartItem 
                { 
                    Id = Guid.NewGuid(), 
                    SeatId = seat.Id, 
                    EventId = Guid.NewGuid(), 
                    CartId = cart.Id, 
                    DateUTC = DateTime.UtcNow 
                };
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _cartService.DeleteSeatFromCartAsync(cart.Id, cartItem.EventId, cartItem.SeatId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task BookCart_ShouldReturnPaymentId()
        {
            // Arrange
            var cart = new Cart { Id = Guid.NewGuid(), UUIDKey = Guid.NewGuid() };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            var seat = new Seat
            {
                Id = Guid.NewGuid(),
                Row = 1,
                Number = 1,
                Price = new Price
                {
                    Name = "VIP",
                    Amount = 100
                },
                Status = SeatStatus.Available
            };
            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();

            var cartItem = new CartItem 
                { 
                    Id = Guid.NewGuid(), 
                    SeatId = seat.Id, 
                    EventId = Guid.NewGuid(), 
                    CartId = cart.Id, 
                    DateUTC = DateTime.UtcNow 
                };
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            // Act
            var paymentId = await _cartService.BookCartAsync(cart.UUIDKey);

            // Assert
            paymentId.Should().NotBeNull();
        }
    }
}