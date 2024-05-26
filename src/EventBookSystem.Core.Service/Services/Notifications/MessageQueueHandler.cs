using Azure.Messaging.ServiceBus;
using EventBookSystem.Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EventBookSystem.Core.Service.Services.Notifications
{
    public class MessageQueueHandler
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MessageQueueHandler> _logger;

        public MessageQueueHandler(IConfiguration configuration, ILogger<MessageQueueHandler> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendMessageAsync(NotificationRequest request)
        {
            var connectionString = _configuration["ServiceBus:ConnectionString"];
            var queueName = _configuration["ServiceBus:QueueName"];

            if (request is null)
            {
                _logger.LogError("Service Bus connection string or queue name is not configured.");

                throw new KeyNotFoundException("Service Bus connection string or queue name is not configured.");
            }

            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(queueName))
            {
                _logger.LogError("Service Bus connection string or queue name is not configured.");

                throw new KeyNotFoundException("Service Bus connection string or queue name is not configured.");
            }

            await using var client = new ServiceBusClient(connectionString);

            var sender = client.CreateSender(queueName);
            var message = new ServiceBusMessage(JsonConvert.SerializeObject(request));

            await sender.SendMessageAsync(message);
        }

        public async Task<NotificationRequest?> ReceiveMessageAsync()
        {
            var connectionString = _configuration["ServiceBus:ConnectionString"];
            var queueName = _configuration["ServiceBus:QueueName"];

            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(queueName))
            {
                _logger.LogError("Service Bus connection string or queue name is not configured.");

                return default;
            }

            await using var client = new ServiceBusClient(connectionString);

            var receiver = client.CreateReceiver(queueName);
            var message = await receiver.ReceiveMessageAsync();

            var body = message.Body.ToString();
            await receiver.CompleteMessageAsync(message);

            return JsonConvert.DeserializeObject<NotificationRequest>(body);
        }
    }
}
