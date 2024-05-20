using EventBookSystem.API;
using EventBookSystem.Common.DTO;
using EventBookSystem.DAL.DataContext;
using EventBookSystem.IntegrationTests.Services.Initialize;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace EventBookSystem.IntegrationTests.Services
{
    public class AuthenticationControllerIntegrationTests : BaseIntegrationTest<Program>
    {
        private readonly MainDBContext _context;

        public AuthenticationControllerIntegrationTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
            _context = factory.Services.CreateScope().ServiceProvider.GetRequiredService<MainDBContext>();
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task Authenticate_ShouldReturnToken()
        {
            // Arrange
            var user = new UserForAuthenticationDto
            {
                UserName = "admin",
                Password = "admin@123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/authentication", user);
            Console.WriteLine(response);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();

            // Assert
            result?.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Authenticate_ShouldReturnBadRequest()
        {
            // Arrange
            var user = new UserForAuthenticationDto
            {
                UserName = "admin123",
                Password = "admin@123"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/authentication", user);

            // Assert
            response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status400BadRequest);
        }
    }
}