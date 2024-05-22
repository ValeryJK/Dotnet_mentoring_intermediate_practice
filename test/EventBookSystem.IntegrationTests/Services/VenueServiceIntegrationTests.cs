using EventBookSystem.API;
using EventBookSystem.Common.DTO;
using EventBookSystem.Core.Service.Services.Interfaces;
using EventBookSystem.DAL.DataContext;
using EventBookSystem.IntegrationTests.Services.Initialize;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace EventBookSystem.IntegrationTests.Services
{
    public class VenueServiceIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly IVenueService _venueService;
        private readonly MainDBContext _context;

        public VenueServiceIntegrationTests(CustomWebApplicationFactory<Program> factory)
        {
            var scope = factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<MainDBContext>();
            _venueService = scope.ServiceProvider.GetRequiredService<IVenueService>();

            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task CreateVenueAsync_ShouldReturnCreatedVenue()
        {
            // Arrange
            var venueDto = new VenueForCreationDto
            {
                Name = "Test Venue",
                Location = "123 Test Street"
            };

            // Act
            var createdVenue = await _venueService.CreateVenueAsync(venueDto);

            // Assert
            createdVenue.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateVenueAsync_ShouldReturnVenueWithCorrectName()
        {
            // Arrange
            var venueDto = new VenueForCreationDto
            {
                Name = "Test Venue",
                Location = "123 Test Street"
            };

            // Act
            var createdVenue = await _venueService.CreateVenueAsync(venueDto);

            // Assert
            createdVenue.Name.Should().Be(venueDto.Name);
        }

        [Fact]
        public async Task CreateVenueAsync_ShouldReturnVenueWithCorrectLocation()
        {
            // Arrange
            var venueDto = new VenueForCreationDto
            {
                Name = "Test Venue",
                Location = "123 Test Street"
            };

            // Act
            var createdVenue = await _venueService.CreateVenueAsync(venueDto);

            // Assert
            createdVenue.Location.Should().Be(venueDto.Location);
        }

        [Fact]
        public async Task CreateVenueAsync_ShouldSaveVenueInDatabase()
        {
            // Arrange
            var venueDto = new VenueForCreationDto
            {
                Name = "Test Venue",
                Location = "123 Test Street"
            };

            // Act
            var createdVenue = await _venueService.CreateVenueAsync(venueDto);

            // Assert
            var venueInDb = await _context.Venues.FindAsync(createdVenue.Id);
            venueInDb.Should().BeEquivalentTo(venueDto, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetVenueByIdAsync_ShouldReturnVenue()
        {
            // Arrange
            var venueDto = new VenueForCreationDto
            {
                Name = "Test Venue",
                Location = "123 Test Street"
            };
            var createdVenue = await _venueService.CreateVenueAsync(venueDto);

            // Act
            var fetchedVenue = await _venueService.GetVenueByIdAsync(createdVenue.Id);

            // Assert
            fetchedVenue.Should().NotBeNull();
        }

        [Fact]
        public async Task GetVenueByIdAsync_ShouldReturnVenueWithCorrectName()
        {
            // Arrange
            var venueDto = new VenueForCreationDto
            {
                Name = "Test Venue",
                Location = "123 Test Street"
            };
            var createdVenue = await _venueService.CreateVenueAsync(venueDto);

            // Act
            var fetchedVenue = await _venueService.GetVenueByIdAsync(createdVenue.Id);

            // Assert
            fetchedVenue?.Name.Should().Be(venueDto.Name);
        }

        [Fact]
        public async Task GetVenueByIdAsync_ShouldReturnVenueWithCorrectLocation()
        {
            // Arrange
            var venueDto = new VenueForCreationDto
            {
                Name = "Test Venue",
                Location = "123 Test Street"
            };
            var createdVenue = await _venueService.CreateVenueAsync(venueDto);

            // Act
            var fetchedVenue = await _venueService.GetVenueByIdAsync(createdVenue.Id);

            // Assert
            fetchedVenue?.Location.Should().Be(venueDto.Location);
        }

        [Fact]
        public async Task UpdateVenueAsync_ShouldNotThrow()
        {
            // Arrange
            var venueDto = new VenueForCreationDto
            {
                Name = "Test Venue",
                Location = "123 Test Street"
            };
            var createdVenue = await _venueService.CreateVenueAsync(venueDto);

            var updatedVenueDto = new VenueForUpdateDto
            {
                Name = "Updated Venue",
                Location = "456 Updated Street"
            };

            // Act
            Func<Task> action = async () => await _venueService.UpdateVenueAsync(createdVenue.Id, updatedVenueDto);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task UpdateVenueAsync_ShouldUpdateName()
        {
            // Arrange
            var venueDto = new VenueForCreationDto
            {
                Name = "Test Venue",
                Location = "123 Test Street"
            };
            var createdVenue = await _venueService.CreateVenueAsync(venueDto);

            var updatedVenueDto = new VenueForUpdateDto
            {
                Name = "Updated Venue",
                Location = "456 Updated Street"
            };

            // Act
            await _venueService.UpdateVenueAsync(createdVenue.Id, updatedVenueDto);

            // Assert
            var updatedVenue = await _venueService.GetVenueByIdAsync(createdVenue.Id);
            updatedVenue?.Name.Should().Be(updatedVenueDto.Name);
        }

        [Fact]
        public async Task UpdateVenueAsync_ShouldUpdateLocation()
        {
            // Arrange
            var venueDto = new VenueForCreationDto
            {
                Name = "Test Venue",
                Location = "123 Test Street"
            };
            var createdVenue = await _venueService.CreateVenueAsync(venueDto);

            var updatedVenueDto = new VenueForUpdateDto
            {
                Name = "Updated Venue",
                Location = "456 Updated Street"
            };

            // Act
            await _venueService.UpdateVenueAsync(createdVenue.Id, updatedVenueDto);

            // Assert
            var updatedVenue = await _venueService.GetVenueByIdAsync(createdVenue.Id);
            updatedVenue?.Location.Should().Be(updatedVenueDto.Location);
        }

        [Fact]
        public async Task DeleteVenueAsync_ShouldNotThrow()
        {
            // Arrange
            var venueDto = new VenueForCreationDto
            {
                Name = "Test Venue",
                Location = "123 Test Street"
            };
            var createdVenue = await _venueService.CreateVenueAsync(venueDto);

            // Act
            Func<Task> action = async () => await _venueService.DeleteVenueAsync(createdVenue.Id);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task DeleteVenueAsync_ShouldRemoveVenue()
        {
            // Arrange
            var venueDto = new VenueForCreationDto
            {
                Name = "Test Venue",
                Location = "123 Test Street"
            };
            var createdVenue = await _venueService.CreateVenueAsync(venueDto);

            // Act
            await _venueService.DeleteVenueAsync(createdVenue.Id);

            // Assert
            var deletedVenue = await _venueService.GetVenueByIdAsync(createdVenue.Id);
            deletedVenue.Should().BeNull();
        }
    }
}