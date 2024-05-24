using EventBookSystem.API.Controllers;
using EventBookSystem.Common.DTO;
using EventBookSystem.Core.Service.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EventBookSystem.Tests.Controllers
{
    public class EventsControllerTests
    {
        private readonly Mock<IEventService> _mockEventService;
        private readonly EventsController _eventsController;

        public EventsControllerTests()
        {
            _mockEventService = new Mock<IEventService>();
            _eventsController = new EventsController(_mockEventService.Object);
        }

        [Fact]
        public async Task GetAllEvents_ShouldReturnOkResult()
        {
            // Arrange
            var events = new List<EventDto>
            {
                new EventDto { Id = Guid.NewGuid(), Name = "Event 1" },
                new EventDto { Id = Guid.NewGuid(), Name = "Event 2" }
            };

            _mockEventService.Setup(service => service.GetAllEventsAsync(false)).ReturnsAsync(events);

            // Act
            var result = await _eventsController.GetAllEvents();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAllEvents_ShouldReturnEvents()
        {
            // Arrange
            var events = new List<EventDto>
            {
                new EventDto { Id = Guid.NewGuid(), Name = "Event 1" },
                new EventDto { Id = Guid.NewGuid(), Name = "Event 2" }
            };

            _mockEventService.Setup(service => service.GetAllEventsAsync(false)).ReturnsAsync(events);

            // Act
            var result = await _eventsController.GetAllEvents();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<List<EventDto>>().Subject;
            returnValue.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetEventByIdAsync_ShouldReturnOkResult()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventDto = new EventDto { Id = eventId, Name = "Event 1" };

            _mockEventService.Setup(service => service.GetEventByIdAsync(eventId, false)).ReturnsAsync(eventDto);

            // Act
            var result = await _eventsController.GetEventByIdAsync(eventId);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetEventByIdAsync_ShouldReturnEvent()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventDto = new EventDto { Id = eventId, Name = "Event 1" };

            _mockEventService.Setup(service => service.GetEventByIdAsync(eventId, false)).ReturnsAsync(eventDto);

            // Act
            var result = await _eventsController.GetEventByIdAsync(eventId);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<EventDto>().Subject;
            returnValue.Id.Should().Be(eventId);
        }

        [Fact]
        public async Task GetEventByIdAsync_ShouldReturnNotFoundResult()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            _mockEventService.Setup(service => service.GetEventByIdAsync(eventId, false)).ReturnsAsync((EventDto?)null);

            // Act
            var result = await _eventsController.GetEventByIdAsync(eventId);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateEventAsync_ShouldReturnOkResult()
        {
            // Arrange
            var eventForCreation = new EventForCreationDto { Name = "Event 1" };
            var createdEvent = new EventDto { Id = Guid.NewGuid(), Name = "Event 1" };

            _mockEventService.Setup(service => service.CreateEventAsync(eventForCreation)).ReturnsAsync(createdEvent);

            // Act
            var result = await _eventsController.CreateEventAsync(eventForCreation);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CreateEventAsync_ShouldReturnCreatedEvent()
        {
            // Arrange
            var eventForCreation = new EventForCreationDto { Name = "Event 1" };
            var createdEvent = new EventDto { Id = Guid.NewGuid(), Name = "Event 1" };

            _mockEventService.Setup(service => service.CreateEventAsync(eventForCreation)).ReturnsAsync(createdEvent);

            // Act
            var result = await _eventsController.CreateEventAsync(eventForCreation);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            var returnValue = okResult.Value.Should().BeAssignableTo<EventDto>().Subject;
            returnValue.Name.Should().Be(eventForCreation.Name);
        }

        [Fact]
        public async Task UpdateEventAsync_ShouldReturnNoContent()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventForUpdate = new EventForUpdateDto { Name = "Updated Event 1" };

            _mockEventService.Setup(service => service.UpdateEventAsync(eventId, eventForUpdate, true)).Returns(Task.CompletedTask);

            // Act
            var result = await _eventsController.UpdateEventAsync(eventId, eventForUpdate);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteEventAsync_ShouldReturnNoContent()
        {
            // Arrange
            var eventId = Guid.NewGuid();

            _mockEventService.Setup(service => service.DeleteEventAsync(eventId, true)).Returns(Task.CompletedTask);

            // Act
            var result = await _eventsController.DeleteEventAsync(eventId);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}