namespace EventBookSystem.Common.DTO
{
    public class CartDto
    {
        public Guid Id { get; set; }

        public decimal TotalPrice => (CartItems?.Any() ?? false) ? CartItems.Sum(x => x.Price) : default;

        public IEnumerable<CartItemDto> CartItems { get; set; } = null!;
    }
}