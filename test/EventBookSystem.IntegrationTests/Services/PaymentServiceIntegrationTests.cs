using AutoMapper;
using EventBookSystem.API;
using EventBookSystem.Core.Service.MappingProfile;
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
        private readonly IMapper _mapper;

        public PaymentServiceIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            var scope = factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<MainDBContext>();
            _paymentService = scope.ServiceProvider.GetRequiredService<IPaymentService>();
            _mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingCoreProfile());
            });

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
            _context.Payments.Add(payment);
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
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // Act
            var fetchedPayment = await _paymentService.GetPaymentByIdAsync(payment.Id);

            // Assert
            fetchedPayment.Should().NotBeNull();
            fetchedPayment?.Id.Should().Be(payment.Id);
        }

        [Fact]
        public async Task CompletePaymentAsync_ShouldReturnTrue()
        {
            // Arrange
            var payment = new Payment
            {
                Status = PaymentStatus.Unpaid,
                PaymentMethod = "VISA",
                DateUTC = DateTime.UtcNow
            };
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

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
                CartId = cart.Id,
                PaymentId = payment.Id
            };
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _paymentService.CompletePaymentAsync(payment.Id);

            // Assert
            result.Should().BeTrue();

            var updatedPayment = await _context.Payments.FindAsync(payment.Id);
            updatedPayment?.Status.Should().Be(PaymentStatus.Paid);

            var updatedSeat = await _context.Seats.FindAsync(seat.Id);
            updatedSeat?.Status.Should().Be(SeatStatus.Sold);
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
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

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
                Status = SeatStatus.Sold
            };
            _context.Seats.Add(seat);
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
            _context.CartItems.Add(cartItem);
            await _context.SaveChangesAsync();

            // Act
            var result = await _paymentService.FailPaymentAsync(payment.Id, false);

            // Assert
            result.Should().BeTrue();

            var updatedPayment = await _context.Payments.FindAsync(payment.Id);
            updatedPayment?.Status.Should().Be(PaymentStatus.Failed);

            var updatedSeat = await _context.Seats.FindAsync(seat.Id);
            updatedSeat?.Status.Should().Be(SeatStatus.Available);
        }
    }
}