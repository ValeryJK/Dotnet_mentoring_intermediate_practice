using EventBookSystem.DAL.Entities.Interfaces;

namespace EventBookSystem.Data.Entities
{
    public class CartItem : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid EventId { get; set; }

        public Guid CartId { get; set; }

        public Guid SeatId { get; set; }

        public Guid? PaymentId { get; set; }

        public string? Description { get; set; }

        public DateTime DateUTC { get; set; }

        public Cart Cart { get; set; } = null!;

        public Seat Seat { get; set; } = null!;
    }
}