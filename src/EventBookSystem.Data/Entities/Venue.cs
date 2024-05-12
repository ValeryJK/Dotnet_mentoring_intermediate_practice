using EventBookSystem.DAL.Entities.Interfaces;
using EventBookSystem.Data.Entities;

namespace EventBookSystem.DAL.Entities
{
    public class Venue : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public required string Location { get; set; }
        public DateTime DateUTC { get; set; } = DateTime.UtcNow;
        public ICollection<Section> Sections { get; set; } = new HashSet<Section>();
    }
}
