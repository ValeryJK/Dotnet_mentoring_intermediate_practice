using EventBookSystem.Core.Service.Services.Notifications;
using Microsoft.Extensions.Logging;

namespace EventBookSystem.BackgroundService
{
    public class NotificationBackgroundService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly NotificationHandler _notificationHandler;
        private readonly ILogger<NotificationBackgroundService> _logger;

        public NotificationBackgroundService(NotificationHandler notificationHandler, ILogger<NotificationBackgroundService> logger)
        {
            _notificationHandler = notificationHandler;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await _notificationHandler.HandleNotificationAsync();

                    await Task.Delay(5000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error.");
            }
        }
    }
}