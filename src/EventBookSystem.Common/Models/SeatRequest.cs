namespace EventBookSystem.Common.Models
{
    public class SeatRequest
    {
        public Guid EventId { get; set; }
        public Guid SeatId { get; set; }
        public Guid PriceId { get; set; }
    }
}
