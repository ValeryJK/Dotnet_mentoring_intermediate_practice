using EventBookSystem.Core.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost("{paymentId}/complete")]
        public async Task<IActionResult> CompletePayment(Guid paymentId)
        {
            var success = await _paymentService.CompletePaymentAsync(paymentId);

            if (success)
            {
                return Ok("Payment completed and seats are marked as sold.");
            }

            return BadRequest("Error.");
        }

        [HttpPost("{paymentId}/failed")]
        public async Task<IActionResult> FailedPayment(Guid paymentId)
        {
            var success = await _paymentService.FailPaymentAsync(paymentId);

            if (success)
            {
                return Ok("Payment failed and seats are marked as available.");
            }

            return BadRequest("Error.");
        }
    }
}