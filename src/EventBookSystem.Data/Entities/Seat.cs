using EventBookSystem.DAL.Entities.Interfaces;
using EventBookSystem.Data.Enums;

namespace EventBookSystem.Data.Entities
{
    public class Seat : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid SectionId { get; set; }
        public Guid PriceId { get; set; }
        public required int Row { get; set; }
        public required int Number { get; set; }
        public SeatStatus Status { get; set; }
        public Section Section { get; set; }
        public Price Price { get; set; }
    }
}
