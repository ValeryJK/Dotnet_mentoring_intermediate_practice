namespace EventBookSystem.Common.DTO
{
    public class VenueDTO
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Location { get; set; }
        public DateTime DateUTC { get; set; }
    }

    public class VenueForCreationDto
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }

    public class VenueForUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
