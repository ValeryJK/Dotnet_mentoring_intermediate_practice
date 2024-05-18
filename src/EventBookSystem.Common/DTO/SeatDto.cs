namespace EventBookSystem.Common.DTO
{
    public class SeatDto
    {
        public Guid SeatId { get; set; }

        public Guid SectionId { get; set; }

        public required int Row { get; set; }

        public required int Number { get; set; }

        public required SeatStatusDto Status { get; set; }

        public required PriceDto Price { get; set; }

    }

    public class SeatStatusDto
    {
        public int Id { get; set; }

        public required string Name { get; set; }
    }

    public class PriceDto
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }
    }
}