using AutoMapper;
using EventBookSystem.API;
using EventBookSystem.Common.Models;
using EventBookSystem.DAL.DataContext;
using EventBookSystem.DAL.Entities;
using EventBookSystem.Data.Entities;
using EventBookSystem.Data.Enums;
using EventBookSystem.IntegrationTests.Services.Initialize;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace EventBookSystem.IntegrationTests.Services
{
    public class OrdersControllerIntegrationTests : BaseIntegrationTest<Program>
    {
        private readonly MainDBContext _context;
        private readonly IMapper _mapper;

        public OrdersControllerIntegrationTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
            _context = factory.Services.CreateScope().ServiceProvider.GetRequiredService<MainDBContext>();
            _mapper = factory.Services.CreateScope().ServiceProvider.GetRequiredService<IMapper>();

            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetCartItemsByCartId_ShouldReturnCartItems()
        {
            // Arrange
            await AuthenticateAsync();

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
                Status = SeatStatus.Available
            };
            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();

            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                SeatId = seat.Id,
                EventId = Guid.NewGuid(),
                DateUTC = DateTime.UtcNow,
                CartId = cart.Id
            };
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.GetAsync($"/orders/carts/{cart.Id}");
            response.EnsureSuccessStatusCode();

            var cartItems = await response.Content.ReadFromJsonAsync<CartItem[]>();

            // Assert
            cartItems.Should().NotBeNull();
            cartItems.Should().ContainSingle();
        }

        [Fact]
        public async Task AddSeatToCart_ShouldReturnUpdatedCart()
        {
            // Arrange
            await AuthenticateAsync();

            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UUIDKey = Guid.NewGuid(),
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            var price = new Price
            {
                Id = Guid.NewGuid(),
                Name = "VIP",
                Amount = 100,
            };
            _context.Prices.Add(price);
            await _context.SaveChangesAsync();

            var seat = new Seat
            {
                Id = Guid.NewGuid(),
                Row = 1,
                Number = 1,
                Status = SeatStatus.Available,
                Price = price,
            };
            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();

            var @event = new Event
            {
                Id= Guid.NewGuid(),
                DateUTC = DateTime.UtcNow,
                Name = "Event 1"
            };
            _context.Events.Add(@event);
            await _context.SaveChangesAsync();
           
            var seatRequest = new SeatRequest
            {
                SeatId = seat.Id,
                EventId = @event.Id,
                PriceId = price.Id,
            };

            // Act
            var response = await _client.PostAsJsonAsync($"/orders/carts/{cart.UUIDKey}", seatRequest);
            response.EnsureSuccessStatusCode();

            var updatedCart = await response.Content.ReadFromJsonAsync<Cart>();

            // Assert
            updatedCart.Should().NotBeNull();
            updatedCart.CartItems.Should().ContainSingle();
        }

        [Fact]
        public async Task DeleteSeatFromCart_ShouldReturnNoContent()
        {
            // Arrange
            await AuthenticateAsync();

            var cart = new Cart
            {
                Id = Guid.NewGuid()
            };
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();

            var seat = new Seat
            {
                Id = Guid.NewGuid(),
                Row = 1,
                Number = 1,
                Status = SeatStatus.Available
            };
            await _context.Seats.AddAsync(seat);
            await _context.SaveChangesAsync();

            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                SeatId = seat.Id,
                EventId = Guid.NewGuid(),
                DateUTC = DateTime.UtcNow,
                CartId = cart.Id
            };
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.DeleteAsync($"/orders/carts/{cart.Id}/events/{cartItem.EventId}/seats/{cartItem.SeatId}");
            response.EnsureSuccessStatusCode();

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task BookCart_ShouldReturnPaymentId()
        {
            // Arrange
            await AuthenticateAsync();

            var cart = new Cart
            {
                Id = Guid.NewGuid(),
                UUIDKey = Guid.NewGuid(),
            };
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();

            var price = new Price
            {
                Id = Guid.NewGuid(),
                Name = "VIP",
                Amount = 100,
            };
            await _context.Prices.AddAsync(price);
            await _context.SaveChangesAsync();

            var seat = new Seat
            {
                Id = Guid.NewGuid(),
                Row = 1,
                Number = 1,
                Status = SeatStatus.Available,
                Price = price
            };
            await _context.Seats.AddAsync(seat);
            await _context.SaveChangesAsync();           

            var cartItem = new CartItem
            {
                Id = Guid.NewGuid(),
                SeatId = seat.Id,
                EventId = Guid.NewGuid(),
                DateUTC = DateTime.UtcNow,
                CartId = cart.Id,
            };
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();

            // Act
            var response = await _client.PostAsync($"/orders/carts/{cart.UUIDKey}/book", null);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<object>();

            // Assert
            result.Should().NotBeNull();
        }
    }
}
