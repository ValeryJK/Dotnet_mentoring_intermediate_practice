namespace EventBookSystem.Common.DTO
{
    public class EventDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateUTC { get; set; }
    }

    public class EventForCreationDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DateUTC { get; set; }
    }

    public class EventForUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime DateUTC { get; set; }
    }
}
