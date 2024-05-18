using EventBookSystem.Common.Models;
using EventBookSystem.Core.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventBookSystem.API.Controllers
{
    [Route("orders/carts")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IServiceManager _services;

        public OrdersController(IServiceManager service) => _services = service;

        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCartItemsByCartId(Guid cartId)
        {
            var cart = await _services.CartService.GetCartItemsByCartId(cartId);

            return cart is null ? NotFound("Cart not found or empty") : Ok(cart);
        }

        [HttpPost("{cartId}")]
        public async Task<IActionResult> AddSeatToCart(Guid cartId, [FromBody] SeatRequest payload)
        {
            if (payload is null)
            {
                return BadRequest("Payload is empty");
            }

            var cart = await _services.CartService.AddSeatToCartAsync(cartId, payload);

            return Ok(cart);
        }

        [HttpDelete("{cartId}/events/{eventId}/seats/{seatId}")]
        public async Task<IActionResult> DeleteSeatFromCart(Guid cartId, Guid eventId, Guid seatId)
        {
            var success = await _services.CartService.DeleteSeatFromCartAsync(cartId, eventId, seatId);

            if (success)
            {
                return NoContent();
            }

            return NotFound($"The seat with id {cartId} in event {eventId} could not be found in the cart {seatId}.");
        }

        [HttpPost("{cartId}/book")]
        public async Task<IActionResult> BookCart(Guid cartId)
        {
            var paymentId = await _services.CartService.BookCartAsync(cartId);

            if (paymentId == null)
            {
                return BadRequest($"Cart with id {cartId} is empty or doesn't exist.");
            }

            return Ok(new { PaymentId = paymentId });
        }
    }
}