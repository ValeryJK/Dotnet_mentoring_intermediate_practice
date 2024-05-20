using EventBookSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventBookSystem.Data.Configutations
{
    public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasData
            (
                new CartItem
                {
                    Id = Guid.Parse("55764999-1eba-48fa-a01f-408575b4fd7c"),
                    CartId = Guid.Parse("207b7a33-1503-4f50-86ea-b4ae6e3c1ed6"),
                    SeatId = Guid.Parse("fc75589a-d320-4cf9-888c-db0e2c800ff1"),
                    EventId = Guid.Parse("fc75589a-d320-4cf9-888c-db0e2c800ff1"),
                    PaymentId = Guid.Parse("5dab4d39-bf3a-4f76-8786-c1933f343d42"),
                    DateUTC = DateTime.UtcNow,
                },
                new CartItem
                {
                    Id = Guid.Parse("c1cde08e-870d-4e39-9f87-f99b03782027"),
                    CartId = Guid.Parse("207b7a33-1503-4f50-86ea-b4ae6e3c1ed6"),
                    SeatId = Guid.Parse("933e12bb-22ef-4024-bfce-410c8a997aea"),
                    EventId = Guid.Parse("fc75589a-d320-4cf9-888c-db0e2c800ff1"),
                    PaymentId = Guid.Parse("5dab4d39-bf3a-4f76-8786-c1933f343d42"),
                    DateUTC = DateTime.UtcNow,
                }
            );
        }
    }
}