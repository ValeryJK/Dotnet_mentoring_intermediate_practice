using EventBookSystem.API.ActionFilters;
using EventBookSystem.Common.Models;
using EventBookSystem.Core.Service.Services.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventBookSystem.API.Controllers
{
    [Route("notifications")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly MessageQueueHandler _queueHandler;

        public NotificationsController(MessageQueueHandler queueHandler)
        {
            _queueHandler = queueHandler;
        }

        [HttpPost]
        [ServiceFilter(typeof(SpecialExceptionFilter))]
        public async Task<IActionResult> Post(NotificationRequest request)
        {
            if (request is null)
            {
                return BadRequest("Notification request is empty.");
            }

            await _queueHandler.SendMessageAsync(request);

            return Ok("Notification request has been sent.");
        }
    }
}