using EventBookSystem.DAL.Entities.Interfaces;
using EventBookSystem.Data.Enums;

namespace EventBookSystem.Data.Entities
{
    public class Payment : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public decimal Amount { get; set; }

        public string? PaymentMethod { get; set; }

        public PaymentStatus Status { get; set; }

        public DateTime DateUTC { get; set; }

        public byte[]? RowVersion { get; set; }
    }
}