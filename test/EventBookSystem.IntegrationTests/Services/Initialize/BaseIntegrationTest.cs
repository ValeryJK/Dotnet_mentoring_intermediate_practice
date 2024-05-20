using EventBookSystem.Common.DTO;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;

namespace EventBookSystem.IntegrationTests.Services.Initialize
{
    public class BaseIntegrationTest<TStartup> : IClassFixture<CustomWebApplicationFactory<TStartup>> where TStartup : class
    {
        protected readonly HttpClient _client;

        public BaseIntegrationTest(CustomWebApplicationFactory<TStartup> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        protected async Task AuthenticateAsync()
        {
            var user = new UserForAuthenticationDto
            {
                UserName = "admin",
                Password = "admin@123"
            };

            var response = await _client.PostAsJsonAsync("/authentication", user);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();

            _client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", result?.Token);
        }

        public class AuthenticationResponse
        {
            public required string Token { get; set; }
        }
    }
}