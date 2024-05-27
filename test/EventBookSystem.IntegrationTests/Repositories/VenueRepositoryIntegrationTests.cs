using EventBookSystem.DAL.Repositories;
using EventBookSystem.IntegrationTests.Repositories.Initialize;
using FluentAssertions;

namespace EventBookSystem.IntegrationTests.Repositories
{
    [Collection("Database collection")]
    public class VenueRepositoryIntegrationTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public VenueRepositoryIntegrationTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllVenuesAsync_ShouldReturnAllVenues()
        {
            //Arrange
            var context = _fixture.GetDbContext();
            _fixture.ClearAndSeed(context);

            var repository = new VenueRepository(context);

            // Act
            var result = await repository.GetAllVenuesAsync(false);

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetVenueByIdAsync_ShouldReturnVenue()
        {
            // Arrange
            var context = _fixture.GetDbContext();

            var repository = new VenueRepository(context);

            _fixture.ClearAndSeed(context);

            var venue = (await repository.GetAllVenuesAsync(false)).First();

            // Act
            var result = await repository.GetVenueByIdAsync(venue.Id, false);

            // Assert
            result.Should().NotBeNull();
        }
    }
}