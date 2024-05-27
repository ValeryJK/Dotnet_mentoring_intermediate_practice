using EventBookSystem.API;
using EventBookSystem.Common.DTO;
using EventBookSystem.DAL.DataContext;
using EventBookSystem.IntegrationTests.Services.Initialize;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace EventBookSystem.IntegrationTests.Services
{
    public class VenuesControllerIntegrationTests : BaseIntegrationTest<Program>
    {
        private readonly MainDBContext _context;

        public VenuesControllerIntegrationTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
            _context = factory.Services.CreateScope().ServiceProvider.GetRequiredService<MainDBContext>();
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAllVenues_ShouldReturnVenues()
        {
            // Arrange
            await AuthenticateAsync();

            var venue = new VenueForCreationDto { Name = "Test Venue" };
            await _client.PostAsJsonAsync("/venues", venue);

            // Act
            var response = await _client.GetAsync("/venues");
            response.EnsureSuccessStatusCode();

            var venues = await response.Content.ReadFromJsonAsync<VenueDto[]>();

            // Assert
            venues.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetVenueById_ShouldReturnVenue()
        {
            // Arrange
            await AuthenticateAsync();

            var venue = new VenueForCreationDto { Name = "Test Venue" };
            var postResponse = await _client.PostAsJsonAsync("/venues", venue);
            postResponse.EnsureSuccessStatusCode();
            var createdVenue = await postResponse.Content.ReadFromJsonAsync<VenueDto>();

            // Act
            var response = await _client.GetAsync($"/venues/{createdVenue?.Id}");
            response.EnsureSuccessStatusCode();

            var fetchedVenue = await response.Content.ReadFromJsonAsync<VenueDto>();

            // Assert
            fetchedVenue.Should().NotBeNull();
            fetchedVenue?.Name.Should().Be(createdVenue?.Name);
        }

        [Fact]
        public async Task GetSectionsByVenue_ShouldReturnSections()
        {
            // Arrange
            await AuthenticateAsync();

            var venue = new VenueForCreationDto { Name = "Test Venue" };
            var postResponse = await _client.PostAsJsonAsync("/venues", venue);
            postResponse.EnsureSuccessStatusCode();
            var createdVenue = await postResponse.Content.ReadFromJsonAsync<VenueDto>();

            // Act
            var response = await _client.GetAsync($"/venues/{createdVenue?.Id}/sections");
            response.EnsureSuccessStatusCode();

            var sections = await response.Content.ReadFromJsonAsync<SectionDto[]>();

            // Assert
            sections.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateVenue_ShouldReturnCreatedVenue()
        {
            // Arrange
            await AuthenticateAsync();

            var venue = new VenueForCreationDto { Name = "Test Venue" };

            // Act
            var response = await _client.PostAsJsonAsync("/venues", venue);
            response.EnsureSuccessStatusCode();

            var createdVenue = await response.Content.ReadFromJsonAsync<VenueDto>();

            // Assert
            createdVenue.Should().NotBeNull();
            createdVenue?.Name.Should().Be(venue.Name);
        }

        [Fact]
        public async Task UpdateVenue_ShouldReturnNoContent()
        {
            // Arrange
            await AuthenticateAsync();

            var venue = new VenueForCreationDto { Name = "Test Venue" };
            var postResponse = await _client.PostAsJsonAsync("/venues", venue);
            postResponse.EnsureSuccessStatusCode();

            var createdVenue = await postResponse.Content.ReadFromJsonAsync<VenueDto>();

            var updatedVenue = new VenueForUpdateDto { Name = "Updated Venue" };

            // Act
            var putResponse = await _client.PutAsJsonAsync($"/venues/{createdVenue?.Id}", updatedVenue);
            putResponse.EnsureSuccessStatusCode();

            // Assert
            putResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/venues/{createdVenue?.Id}");
            var fetchedVenue = await getResponse.Content.ReadFromJsonAsync<VenueDto>();
            fetchedVenue?.Name.Should().Be(updatedVenue.Name);
        }

        [Fact]
        public async Task DeleteVenue_ShouldReturnNoContent()
        {
            // Arrange
            await AuthenticateAsync();

            var venue = new VenueForCreationDto { Name = "Test Venue" };
            var postResponse = await _client.PostAsJsonAsync("/venues", venue);
            postResponse.EnsureSuccessStatusCode();
            var createdVenue = await postResponse.Content.ReadFromJsonAsync<VenueDto>();

            // Act
            var deleteResponse = await _client.DeleteAsync($"/venues/{createdVenue?.Id}");
            deleteResponse.EnsureSuccessStatusCode();

            // Assert
            deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/venues/{createdVenue?.Id}");
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}