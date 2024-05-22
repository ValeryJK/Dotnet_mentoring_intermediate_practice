using EventBookSystem.API.Controllers;
using EventBookSystem.Common.DTO;
using EventBookSystem.Core.Service.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace EventBookSystem.Tests.Controllers
{
    public class EventsControllerTests
    {
        private readonly Mock<IEventService> _mockEventService;
        private readonly EventsController _eventsController;
        private readonly IMemoryCache _memoryCache;

        public EventsControllerTests()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _mockEventService = new Mock<IEventService>();
            _eventsController = new EventsController(_mockEventService.Object, _memoryCache);
        }

        [Fact]
        public async Task GetAllEvents_ShouldReturnOkResultWithEvents()
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
        public async Task GetEventByIdAsync_ShouldReturnOkResultWithEvent()
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
        public async Task CreateEventAsync_ShouldReturnOkResultWithCreatedEvent()
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

        [Fact]
        public async Task GetAllEvents_ShouldCacheData()
        {
            // Arrange
            var testEvents = new List<EventDto> { new EventDto { Id = Guid.NewGuid(), Name = "Test Event" } };
            _mockEventService.Setup(s => s.GetAllEventsAsync(false)).ReturnsAsync(testEvents);

            // Act
            var result1 = await _eventsController.GetAllEvents() as OkObjectResult;
            var result2 = await _eventsController.GetAllEvents() as OkObjectResult;

            // Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.Equal(testEvents, result1.Value);
            Assert.Equal(testEvents, result2.Value);
            _mockEventService.Verify(s => s.GetAllEventsAsync(false), Times.Once);
        }

        [Fact]
        public async Task CreateEvent_ShouldInvalidateCache()
        {
            // Arrange
            var newEvent = new EventForCreationDto { Name = "New Event" };
            var createdEvent = new EventDto { Id = Guid.NewGuid(), Name = "New Event" };
            _mockEventService.Setup(s => s.CreateEventAsync(newEvent)).ReturnsAsync(createdEvent);

            // Act
            var result = await _eventsController.CreateEventAsync(newEvent);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var cachedEvents = _memoryCache.Get<List<EventDto>>("AllEvents");
            Assert.Null(cachedEvents); 
        }

        [Fact]
        public async Task UpdateEvent_ShouldInvalidateCache()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var updatedEvent = new EventForUpdateDto { Name = "Updated Event" };
            _mockEventService.Setup(s => s.UpdateEventAsync(eventId, updatedEvent, true)).Returns(Task.CompletedTask);

            // Act
            var result = await _eventsController.UpdateEventAsync(eventId, updatedEvent);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var cachedEvent = _memoryCache.Get<EventDto>($"Event_{eventId}");
            Assert.Null(cachedEvent);
        }
    }
}