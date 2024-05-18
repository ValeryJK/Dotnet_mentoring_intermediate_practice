namespace EventBookSystem.Common.DTO
{
    public class SectionDto
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public int Capacity { get; set; }

        public DateTime DateUTC { get; set; }
    }
}