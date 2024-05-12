namespace EventBookSystem.Common.DTO
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public decimal TotalPrice => CartItems.Sum(x => x.Price);
        public IEnumerable<CartItemDto> CartItems { get; set; }
    }
}
