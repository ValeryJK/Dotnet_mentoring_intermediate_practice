using EventBookSystem.API;
using EventBookSystem.Common.DTO;
using EventBookSystem.DAL.DataContext;
using EventBookSystem.IntegrationTests.Services.Initialize;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Json;

namespace EventBookSystem.IntegrationTests.Services
{
    public class EventsControllerIntegrationTests : BaseIntegrationTest<Program>
    {
        private readonly MainDBContext _context;

        public EventsControllerIntegrationTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
            _context = factory.Services.CreateScope().ServiceProvider.GetRequiredService<MainDBContext>();
            _context.Database.EnsureCreated();
        }

        [Fact]
        public async Task GetAllEvents_ShouldReturnEvents()
        {
            // Arrange
            await AuthenticateAsync();

            var eventDto = new EventForCreationDto { Name = "Test Event" };
            await _client.PostAsJsonAsync("/events", eventDto);

            // Act
            var response = await _client.GetAsync("/events");
            response.EnsureSuccessStatusCode();

            var events = await response.Content.ReadFromJsonAsync<EventDto[]>();

            // Assert
            events.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetEventById_ShouldReturnEvent()
        {
            // Arrange
            await AuthenticateAsync();

            var eventDto = new EventForCreationDto { Name = "Test Event" };
            var postResponse = await _client.PostAsJsonAsync("/events", eventDto);
            postResponse.EnsureSuccessStatusCode();
            var createdEvent = await postResponse.Content.ReadFromJsonAsync<EventDto>();

            // Act
            var response = await _client.GetAsync($"/events/{createdEvent?.Id}");
            response.EnsureSuccessStatusCode();

            var fetchedEvent = await response.Content.ReadFromJsonAsync<EventDto>();

            // Assert
            fetchedEvent.Should().NotBeNull();
            fetchedEvent?.Id.Should().Be(createdEvent.Id);
            fetchedEvent?.Name.Should().Be(createdEvent.Name);
        }

        [Fact]
        public async Task GetSeatsBySection_ShouldReturnSeats()
        {
            // Arrange
            await AuthenticateAsync();

            var eventDto = new EventForCreationDto { Name = "Test Event" };
            var postResponse = await _client.PostAsJsonAsync("/events", eventDto);
            postResponse.EnsureSuccessStatusCode();

            var createdEvent = await postResponse.Content.ReadFromJsonAsync<EventDto>();

            var sectionId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"/events/{createdEvent?.Id}/sections/{sectionId}/seats");
            response.EnsureSuccessStatusCode();

            var seats = await response.Content.ReadFromJsonAsync<SeatDto[]>();

            // Assert
            seats.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateEvent_ShouldReturnCreatedEvent()
        {
            // Arrange
            await AuthenticateAsync();

            var eventDto = new EventForCreationDto { Name = "Test Event" };

            // Act
            var response = await _client.PostAsJsonAsync("/events", eventDto);
            response.EnsureSuccessStatusCode();

            var createdEvent = await response.Content.ReadFromJsonAsync<EventDto>();

            // Assert
            createdEvent.Should().NotBeNull();
            createdEvent?.Name.Should().Be(eventDto.Name);
        }

        [Fact]
        public async Task UpdateEvent_ShouldReturnNoContent()
        {
            // Arrange
            await AuthenticateAsync();

            var eventDto = new EventForCreationDto { Name = "Test Event" };
            var postResponse = await _client.PostAsJsonAsync("/events", eventDto);

            postResponse.EnsureSuccessStatusCode();

            var createdEvent = await postResponse.Content.ReadFromJsonAsync<EventDto>();
            var updatedEventDto = new EventForUpdateDto { Name = "Updated Event" };

            // Act
            var putResponse = await _client.PutAsJsonAsync($"/events/{createdEvent?.Id}", updatedEventDto);
            putResponse.EnsureSuccessStatusCode();

            // Assert
            putResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/events/{createdEvent?.Id}");
            var fetchedEvent = await getResponse.Content.ReadFromJsonAsync<EventDto>();
            fetchedEvent?.Name.Should().Be(updatedEventDto.Name);
        }

        [Fact]
        public async Task DeleteEvent_ShouldReturnNoContent()
        {
            // Arrange
            await AuthenticateAsync();

            var eventDto = new EventForCreationDto { Name = "Test Event" };
            var postResponse = await _client.PostAsJsonAsync("/events", eventDto);

            postResponse.EnsureSuccessStatusCode();

            var createdEvent = await postResponse.Content.ReadFromJsonAsync<EventDto>();

            // Act
            var deleteResponse = await _client.DeleteAsync($"/events/{createdEvent?.Id}");
            deleteResponse.EnsureSuccessStatusCode();

            // Assert
            deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/events/{createdEvent.Id}");
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}