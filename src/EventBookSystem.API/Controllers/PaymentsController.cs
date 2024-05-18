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
        private readonly IServiceManager _services;

        public PaymentsController(IServiceManager service) => _services = service;

        /// <summary>
        /// Get the list of all payments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _services.PaymentService.GetAllPaymentsAsync();

            return Ok(payments);
        }

        /// <summary>
        /// Returns payment by id
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        [HttpGet("{paymentId}")]
        public async Task<IActionResult> GetPaymentByIdAsync(Guid paymentId)
        {
            var sections = await _services.PaymentService.GetPaymentByIdAsync(paymentId);

            return Ok(sections);
        }

        /// <summary>
        /// Updates payment status and moves all the seats related to a payment to the sold state.
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        [HttpPost("{paymentId}/complete")]
        public async Task<IActionResult> CompletePayment(Guid paymentId)
        {
            var success = await _services.PaymentService.CompletePaymentAsync(paymentId);

            if (!success)
            {
                return BadRequest("Error.");
            }

            return Ok("Payment completed successfully and seats are marked as sold.");
        }

        /// <summary>
        /// Updates payment status and moves all the seats related to a payment to the available state.
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns></returns>
        [HttpPost("{paymentId}/failed")]
        public async Task<IActionResult> FailedPayment(Guid paymentId)
        {
            var success = await _services.PaymentService.FailPaymentAsync(paymentId);

            if (!success)
            {
                return BadRequest("Error.");
            }

            return Ok("Payment failed successfully and seats are marked as available.");
        }
    }
}