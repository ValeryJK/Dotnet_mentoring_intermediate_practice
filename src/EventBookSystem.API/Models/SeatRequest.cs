namespace EventBookSystem.API.Models
{
    public class SeatRequest
    {
        public int EventId { get; set; }
        public int SeatId { get; set; }
        public int PriceId { get; set; }
    }
}
