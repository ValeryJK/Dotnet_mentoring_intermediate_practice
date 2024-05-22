using EventBookSystem.API;
using EventBookSystem.Core.Service.Services.Interfaces;
using EventBookSystem.DAL.DataContext;
using EventBookSystem.Data.Entities;
using EventBookSystem.Data.Enums;
using EventBookSystem.IntegrationTests.Services.Initialize;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EventBookSystem.IntegrationTests.Services
{
    public class PaymentServiceIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly IPaymentService _paymentService;
        private readonly MainDBContext _context;

        public PaymentServiceIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            var scope = factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<MainDBContext>();
            _paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();

            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAllPaymentsAsync_ShouldReturnPayments()
        {
            // Arrange
            var payment = new Payment
            {
                Status = PaymentStatus.Paid,
                PaymentMethod = "VISA",
                DateUTC = DateTime.UtcNow
            };
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            // Act
            var payments = await _paymentService.GetAllPaymentsAsync();

            // Assert
            payments.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetPaymentByIdAsync_ShouldReturnPayment()
        {
            // Arrange            
            var payment = new Payment
            {
                Status = PaymentStatus.Paid,
                PaymentMethod = "VISA",
                DateUTC = DateTime.UtcNow
            };
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            // Act
            var fetchedPayment = await _paymentService.GetPaymentByIdAsync(payment.Id);

            // Assert
            fetchedPayment.Should().NotBeNull();
        }

        [Fact]
        public async Task FailPaymentAsync_ShouldReturnTrue()
        {
            // Arrange
            var payment = new Payment
            {
                Status = PaymentStatus.Unpaid,
                PaymentMethod = "VISA",
                DateUTC = DateTime.UtcNow
            };
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

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
                Status = SeatStatus.Sold
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
                PaymentId = payment.Id
            };
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _paymentService.FailPaymentAsync(payment.Id, false);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task FailPaymentAsync_ShouldUpdatePaymentStatusToFailed()
        {
            // Arrange
            var payment = new Payment
            {
                Status = PaymentStatus.Unpaid,
                PaymentMethod = "VISA",
                DateUTC = DateTime.UtcNow
            };
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

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
                Status = SeatStatus.Sold
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
                PaymentId = payment.Id
            };
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();

            // Act
            await _paymentService.FailPaymentAsync(payment.Id, false);

            // Assert
            var updatedPayment = await _context.Payments.FindAsync(payment.Id);
            updatedPayment?.Status.Should().Be(PaymentStatus.Failed);
        }

        [Fact]
        public async Task FailPaymentAsync_ShouldUpdateSeatStatusToAvailable()
        {
            // Arrange
            var payment = new Payment
            {
                Status = PaymentStatus.Unpaid,
                PaymentMethod = "VISA",
                DateUTC = DateTime.UtcNow
            };
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

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
                Status = SeatStatus.Sold
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
                PaymentId = payment.Id
            };
            await _context.CartItems.AddAsync(cartItem);
            await _context.SaveChangesAsync();

            // Act
            await _paymentService.FailPaymentAsync(payment.Id, false);

            // Assert
            var updatedSeat = await _context.Seats.FindAsync(seat.Id);
            updatedSeat?.Status.Should().Be(SeatStatus.Available);
        }
    }
}