using EventBookSystem.DAL.Entities.Interfaces;

namespace EventBookSystem.Data.Entities
{
    public class Price : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public decimal Amount { get; set; }
        public ICollection<Seat> Seats { get; set; } = new HashSet<Seat>();
    }
}
