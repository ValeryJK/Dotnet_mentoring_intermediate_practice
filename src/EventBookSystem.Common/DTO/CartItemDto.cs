namespace EventBookSystem.Common.DTO
{
    public class CartItemDto
    {
        public Guid Id { get; set; }

        public Guid EventId { get; set; }

        public Guid CartId { get; set; }

        public Guid SeatId { get; set; }

        public string? Description { get; set; }

        public string? Status { get; set; }

        public DateTime DateUTC { get; set; }

        public decimal Price { get; set; }
    }
}