using EventBookSystem.Data.Entities;
using EventBookSystem.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventBookSystem.Data.Configutations
{
    public class SeatConfiguration : IEntityTypeConfiguration<Seat>
    {
        public void Configure(EntityTypeBuilder<Seat> builder)
        {
            builder.HasData
            (
                new Seat
                {
                    Id = Guid.Parse("fc75589a-d320-4cf9-888c-db0e2c800ff1"),
                    SectionId = Guid.Parse("58ce05b1-d34c-4a3e-a309-536d3b74be8b"),
                    PriceId = Guid.Parse("9216f308-9c22-4bae-a17e-1763c0d62234"),
                    Number = 10,
                    Row = 20,
                    Status = SeatStatus.Booked,
                },
                new Seat
                {
                    Id = Guid.Parse("76045c78-a4c9-4a66-a1c7-a39490dcc3dc"),
                    SectionId = Guid.Parse("3f14e23b-98e2-4646-8feb-180108ca17c5"),
                    PriceId = Guid.Parse("9216f308-9c22-4bae-a17e-1763c0d62234"),
                    Number = 20,
                    Row = 30,
                    Status = SeatStatus.Sold,
                },
                new Seat
                {
                    Id = Guid.Parse("933e12bb-22ef-4024-bfce-410c8a997aea"),
                    SectionId = Guid.Parse("3f14e23b-98e2-4646-8feb-180108ca17c5"),
                    PriceId = Guid.Parse("9216f308-9c22-4bae-a17e-1763c0d62234"),
                    Number = 21,
                    Row = 30,
                    Status = SeatStatus.Booked,
                },
                new Seat
                {
                    Id = Guid.Parse("7ae65024-1c96-4e2c-82d3-a81fa6e634df"),
                    SectionId = Guid.Parse("3f14e23b-98e2-4646-8feb-180108ca17c5"),
                    PriceId = Guid.Parse("9216f308-9c22-4bae-a17e-1763c0d62234"),
                    Number = 21,
                    Row = 40,
                    Status = SeatStatus.Available,
                }
            );
        }
    }
}