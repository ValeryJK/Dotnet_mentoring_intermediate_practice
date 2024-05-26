using EventBookSystem.Core.Service.Services.Email;
using Polly;
using SendGrid;

namespace EventBookSystem.Core.Service.Services.Notifications
{
    public class NotificationHandler
    {
        private readonly MessageQueueHandler _queueHandler;
        private readonly EmailService _emailService;

        public NotificationHandler(MessageQueueHandler queueHandler, EmailService emailService)
        {
            _queueHandler = queueHandler;
            _emailService = emailService;
        }

        public async Task HandleNotificationAsync()
        {
            var notification = await _queueHandler.ReceiveMessageAsync();

            if (notification is null)
            {
                Console.WriteLine("No message received from the queue.");

                return;
            }

            var response = await _emailService.SendEmailAsync(notification);

            if (!response.IsSuccessStatusCode)
            {
                var retryPolicy = Policy
                    .HandleResult<Response>(r => !r.IsSuccessStatusCode)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2));

                await retryPolicy.ExecuteAsync(() => _emailService.SendEmailAsync(notification));
            }
        }
    }
}