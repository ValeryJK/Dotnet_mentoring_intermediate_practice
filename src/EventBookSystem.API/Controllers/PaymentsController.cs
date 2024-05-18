using EventBookSystem.Common.DTO;
using EventBookSystem.Core.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentStatus = EventBookSystem.Common.DTO.PaymentStatus;

namespace EventBookSystem.API.Controllers
{
    [Route("payments")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize]
    public class PaymentsController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService) => _paymentService = paymentService;

        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _paymentService.GetAllPaymentsAsync();

            return Ok(payments);
        }

        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPaymentByIdAsync(Guid paymentId)
        {
            var sections = await _paymentService.GetPaymentByIdAsync(paymentId);

            return Ok(sections);
        }

        [HttpPost("{paymentId}/status")]
        public async Task<IActionResult> UpdatePaymentStatus(Guid paymentId, [FromBody] UpdatePaymentStatusDto updatePaymentStatusDto)
        {
            if (updatePaymentStatusDto is null)
            {
                return BadRequest("Invalid update payment status.");
            }

            bool success;
            switch (updatePaymentStatusDto.Status)
            {
                case PaymentStatus.Complete:
                    success = await _paymentService.CompletePaymentAsync(paymentId);
                    break;
                case PaymentStatus.Failed:
                    success = await _paymentService.FailPaymentAsync(paymentId);
                    break;
                default:
                    return BadRequest("Invalid status value.");
            }

            if (success)
            {
                return Ok($"Payment {updatePaymentStatusDto.Status.ToString().ToLower()} successfully and seats are updated accordingly.");
            }

            return BadRequest("Error updating payment status.");
        }
    }
}