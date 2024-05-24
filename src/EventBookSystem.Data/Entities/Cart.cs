using EventBookSystem.DAL.Entities.Interfaces;

namespace EventBookSystem.Data.Entities
{
    public class Cart : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid? UserId { get; set; }

        public Guid UUIDKey { get; set; }

        public string? Description { get; set; }

        public DateTime DateUTC { get; set; }

        public byte[]? RowVersion { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new HashSet<CartItem>();
    }
}