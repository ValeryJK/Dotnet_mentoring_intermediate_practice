using AutoMapper;
using EventBookSystem.Common.DTO;
using EventBookSystem.Core.Service.MappingProfile;
using EventBookSystem.Core.Service.Services;
using EventBookSystem.DAL.Entities;
using EventBookSystem.DAL.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace EventBookSystem.Tests.Services
{
    public class EventServiceTests
    {
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly Mock<ILogger<EventService>> _mockLogger;
        private readonly IMapper _mapper;
        private readonly EventService _eventService;

        public EventServiceTests()
        {
            _mockEventRepository = new Mock<IEventRepository>();
            _mockLogger = new Mock<ILogger<EventService>>();


            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingCoreProfile());
            });

            _mapper = mappingConfig.CreateMapper();
            _eventService = new EventService(_mockEventRepository.Object, _mockLogger.Object, _mapper);
        }

        [Fact]
        public async Task GetAllEventsAsync_ShouldReturnAllEvents()
        {
            // Arrange
            var events = new List<Event>
            {
                new Event { Id = Guid.NewGuid(), Name = "Event 1" },
                new Event { Id = Guid.NewGuid(), Name = "Event 2" }
            };

            _mockEventRepository.Setup(repo => repo.GetAllEventsAsync(false)).ReturnsAsync(events);

            // Act
            var result = await _eventService.GetAllEventsAsync(false);

            // Assert
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetEventByIdAsync_ShouldReturnNotNull()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventItem = new Event { Id = eventId, Name = "Event 1" };

            _mockEventRepository.Setup(repo => repo.GetEventByIdAsync(eventId, false)).ReturnsAsync(eventItem);

            // Act
            var result = await _eventService.GetEventByIdAsync(eventId, false);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetEventByIdAsync_ShouldReturnCorrectId()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventItem = new Event { Id = eventId, Name = "Event 1" };

            _mockEventRepository.Setup(repo => repo.GetEventByIdAsync(eventId, false)).ReturnsAsync(eventItem);

            // Act
            var result = await _eventService.GetEventByIdAsync(eventId, false);

            // Assert
            result!.Id.Should().Be(eventId);
        }

        [Fact]
        public async Task CreateEventAsync_ShouldReturnNotNull()
        {
            // Arrange
            var eventForCreation = new EventForCreationDto { Name = "Event 1" };

            _mockEventRepository.Setup(repo => repo.Create(It.IsAny<Event>()));
            _mockEventRepository.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _eventService.CreateEventAsync(eventForCreation);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateEventAsync_ShouldReturnCorrectName()
        {
            // Arrange
            var eventForCreation = new EventForCreationDto { Name = "Event 1" };

            _mockEventRepository.Setup(repo => repo.Create(It.IsAny<Event>()));
            _mockEventRepository.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _eventService.CreateEventAsync(eventForCreation);

            // Assert
            result!.Name.Should().Be(eventForCreation.Name);
        }

        [Fact]
        public async Task UpdateEventAsync_ShouldNotThrow()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventForUpdate = new EventForUpdateDto { Name = "Updated Event 1" };
            var eventEntity = new Event { Id = eventId, Name = "Event 1" };

            _mockEventRepository.Setup(repo => repo.GetEventByIdAsync(eventId, false)).ReturnsAsync(eventEntity);
            _mockEventRepository.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            Func<Task> action = async () => await _eventService.UpdateEventAsync(eventId, eventForUpdate, false);

            // Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task UpdateEventAsync_ShouldUpdateName()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventForUpdate = new EventForUpdateDto { Name = "Updated Event 1" };
            var eventEntity = new Event { Id = eventId, Name = "Event 1" };

            _mockEventRepository.Setup(repo => repo.GetEventByIdAsync(eventId, false)).ReturnsAsync(eventEntity);
            _mockEventRepository.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            await _eventService.UpdateEventAsync(eventId, eventForUpdate, false);

            // Assert
            eventEntity.Name.Should().Be(eventForUpdate.Name);
        }

        [Fact]
        public async Task DeleteEventAsync_ShouldNotThrow()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            var eventEntity = new Event { Id = eventId, Name = "Event 1" };

            _mockEventRepository.Setup(repo => repo.GetEventByIdAsync(eventId, false)).ReturnsAsync(eventEntity);
            _mockEventRepository.Setup(repo => repo.Delete(It.IsAny<Event>()));
            _mockEventRepository.Setup(repo => repo.SaveAsync()).Returns(Task.CompletedTask);

            // Act
            Func<Task> action = async () => await _eventService.DeleteEventAsync(eventId, false);

            // Assert
            await action.Should().NotThrowAsync();
        }
    }
}