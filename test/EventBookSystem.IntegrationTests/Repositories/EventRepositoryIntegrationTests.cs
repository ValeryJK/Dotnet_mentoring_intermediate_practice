using EventBookSystem.DAL.Repositories;
using EventBookSystem.IntegrationTests.Repositories.Initialize;
using FluentAssertions;

namespace EventBookSystem.IntegrationTests.Repositories
{
    [Collection("Database collection")]
    public class EventRepositoryIntegrationTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;

        public EventRepositoryIntegrationTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllEventsAsync_ShouldReturnAllEvents()
        {
            // Arrange
            var context = _fixture.GetDbContext();
            _fixture.ClearAndSeed(context);

            var repository = new EventRepository(context);

            // Act
            var events = await repository.GetAllEventsAsync(false);

            // Assert
            events.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetSeatsBySection_ShouldReturnSeatsForGivenSection()
        {
            // Arrange
            var context = _fixture.GetDbContext();
            _fixture.ClearAndSeed(context);

            var repository = new EventRepository(context);

            var section = context.Sections.First();

            // Act
            var seats = await repository.GetSeatsBySection(section.EventId, section.Id, false);

            // Assert
            seats.Should().NotBeNull();
        }

        [Fact]
        public async Task GetEventByIdAsync_ShouldReturnEventWithSections()
        {
            // Arrange
            var context = _fixture.GetDbContext();
            _fixture.ClearAndSeed(context);

            var repository = new EventRepository(context);

            var eventId = context.Events.First().Id;

            // Act
            var result = await repository.GetEventByIdAsync(eventId, false);

            // Assert
            result.Should().NotBeNull();
        }
    }
}