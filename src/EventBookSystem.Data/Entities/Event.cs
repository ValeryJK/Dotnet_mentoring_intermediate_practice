using EventBookSystem.DAL.Entities.Interfaces;
using EventBookSystem.Data.Entities;

namespace EventBookSystem.DAL.Entities
{
    public class Event : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateUTC { get; set; }
        public ICollection<Section> Sections { get; set; }
    }
}
