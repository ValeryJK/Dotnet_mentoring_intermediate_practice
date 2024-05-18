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

        /// <summary>
        /// Gets list of items in a cart (cart_id is a uuid, generated and stored the client side)
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCartItemsByCartId(Guid cartId)
        {
            var cart = await _services.CartService.GetCartItemsByCartId(cartId, false);

            if (cart == null)
                return NotFound("Cart not found or empty");

            return Ok(cart);
        }

        /// <summary>
        ///  Takes object of event_id, seat_id and price_id as a payload and adds a seat to the cart. 
        ///  Returns a cart state (with total amount) back to the caller)
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        [HttpPost("{cartId}")]
        public async Task<IActionResult> AddSeatToCart(Guid cartId, [FromBody] SeatRequest payload)
        {
            if (payload is null)
                return BadRequest("Payload is empty");

            var cart = await _services.CartService.AddSeatToCartAsync(cartId, payload);

            return Ok(cart);
        }

        /// <summary>
        /// Deletes a seat for a specific cart
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="eventId"></param>
        /// <param name="seatId"></param>
        /// <returns></returns>
        [HttpDelete("{cartId}/events/{eventId}/seats/{seatId}")]
        public async Task<IActionResult> DeleteSeatFromCart(Guid cartId, Guid eventId, Guid seatId)
        {
            var success = await _services.CartService.DeleteSeatFromCartAsync(cartId, eventId, seatId);

            if (!success)
            {
                return NotFound($"The seat with id {cartId} in event {eventId} could not be found in the cart {seatId}.");
            }

            return NoContent();
        }

        /// <summary>
        /// Moves all the seats in the cart to a booked state. Returns a PaymentId.
        /// </summary>
        /// <param name="cartId"></param>
        /// <returns></returns>
        [HttpPut("{cartId}/book")]
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