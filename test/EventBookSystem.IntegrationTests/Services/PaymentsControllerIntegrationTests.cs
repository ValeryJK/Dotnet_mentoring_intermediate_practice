using EventBookSystem.API;
using EventBookSystem.Common.DTO;
using EventBookSystem.DAL.DataContext;
using EventBookSystem.IntegrationTests.Services.Initialize;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace EventBookSystem.IntegrationTests.Services
{
    public class PaymentsControllerIntegrationTests : BaseIntegrationTest<Program>
    {
        private readonly MainDBContext _context;

        public PaymentsControllerIntegrationTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
            _context = factory.Services.CreateScope().ServiceProvider.GetRequiredService<MainDBContext>();
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAllPayments_ShouldReturnPayments()
        {
            // Arrange
            await AuthenticateAsync();

            // Act
            var response = await _client.GetAsync("/payments");
            response.EnsureSuccessStatusCode();

            var payments = await response.Content.ReadFromJsonAsync<PaymentDto[]>();

            // Assert
            payments.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetPaymentById_ShouldReturnPayment()
        {
            // Arrange
            await AuthenticateAsync();

            var response1 = await _client.GetAsync("/payments");
            response1.EnsureSuccessStatusCode();

            var payment = (await response1.Content.ReadFromJsonAsync<PaymentDto[]>())?.First();

            // Act
            var response2 = await _client.GetAsync($"/payments/{payment?.Id}");
            response2.EnsureSuccessStatusCode();

            var fetchedPayment = await response2.Content.ReadFromJsonAsync<PaymentDto>();

            // Assert
            fetchedPayment.Should().NotBeNull();
            fetchedPayment?.Id.Should().Be(payment.Id);
        }

        [Fact]
        public async Task UpdatePaymentStatus_ShouldReturnSuccessMessage()
        {
            // Arrange
            await AuthenticateAsync();

            var response1 = await _client.GetAsync("/payments");
            response1.EnsureSuccessStatusCode();

            var payment = (await response1.Content.ReadFromJsonAsync<PaymentDto[]>())?.First();

            var updatePaymentStatusDto = new UpdatePaymentStatusDto
            {
                Status = PaymentStatus.Complete
            };

            // Act
            var response = await _client.PostAsJsonAsync($"/payments/{payment?.Id}/status", updatePaymentStatusDto);
            response.EnsureSuccessStatusCode();

            var responseMessage = await response.Content.ReadAsStringAsync();

            // Assert
            responseMessage.Should().Contain("Payment complete successfully and seats are updated accordingly.");
        }

    }
}