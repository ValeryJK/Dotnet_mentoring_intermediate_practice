using EventBookSystem.Common.Models;
using SendGrid;

namespace EventBookSystem.Core.Service.Services.Interfaces
{
    public interface IEmailService
    {
        Task<Response> SendEmailAsync(NotificationRequest notification);
    }
}