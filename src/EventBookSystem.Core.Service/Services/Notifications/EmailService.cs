using EventBookSystem.Common.Models;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace EventBookSystem.Core.Service.Services.Email
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Response> SendEmailAsync(NotificationRequest notification)
        {
            var apiKey = _configuration["SendGrid:ApiKey"];

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("SendGrid apiKey is not configured.");
            }

            var client = new SendGridClient(apiKey);

            var from = new EmailAddress("no-reply@yourdomain.com", "TestService");
            var to = new EmailAddress(notification.CustomerEmail, notification.CustomerName);
            var subject = notification.OperationName;
            var plainTextContent = notification.Content;
            var htmlContent = $"<strong>{notification.Content}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);

            var statusCode = response.StatusCode;
            var responseBody = await response.Body.ReadAsStringAsync();

            Console.WriteLine($"SendGrid Response Status Code: {statusCode}");
            Console.WriteLine($"SendGrid Response Body: {responseBody}");

            return response;
        }
    }
}