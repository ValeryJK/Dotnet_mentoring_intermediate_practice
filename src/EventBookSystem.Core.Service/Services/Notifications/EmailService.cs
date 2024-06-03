using EventBookSystem.Common.Common;
using EventBookSystem.Common.Models;
using EventBookSystem.Core.Service.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace EventBookSystem.Core.Service.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly SendGridSettings _sendGridSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<SendGridSettings> sendGridSettings, ILogger<EmailService> logger)
        {
            _sendGridSettings = sendGridSettings.Value;
            _logger = logger;
        }

        public async Task<Response> SendEmailAsync(NotificationRequest notification)
        {
            var apiKey = _sendGridSettings.ApiKey;

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("SendGrid apiKey is not configured.");
            }

            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(notification.From, "TestService");
            var to = new EmailAddress(notification.To, notification.CustomerName);
            var subject = notification.OperationName;
            var plainTextContent = notification.Content;
            var htmlContent = $"<strong>{notification.Content}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

            var statusCode = response.StatusCode;
            var responseBody = await response.Body.ReadAsStringAsync();

            _logger.LogDebug("SendGrid Response Status Code: {StatusCode}", statusCode);
            _logger.LogDebug("SendGrid Response Body: {ResponseBody}", responseBody);

            return response;
        }
    }
}