using EventBookSystem.DAL.Entities;
using EventBookSystem.DAL.Entities.Interfaces;

namespace EventBookSystem.Data.Entities
{
    public class Section : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid EventId { get; set; }
        public Guid VenueId { get; set; }
        public required string Name { get; set; }
        public int Capacity { get; set; }
        public DateTime DateUTC { get; set; }
        public Event Event { get; set; }
        public Venue Venue { get; set; }
        public ICollection<Seat> Seats { get; set; } = new HashSet<Seat>();
    }
}
