using EventBookSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventBookSystem.Data.Configutations
{
    public class PriceConfiguration : IEntityTypeConfiguration<Price>
    {
        public void Configure(EntityTypeBuilder<Price> builder)
        {
            builder.HasData
            (
                new Price
                {
                    Id = Guid.Parse("9216f308-9c22-4bae-a17e-1763c0d62234"),
                    Name = "VIP",
                    Amount = 100,
                },
                new Price
                {
                    Id = Guid.Parse("623643cf-3f59-4dac-bc07-cfb53b51e669"),
                    Name = "Child",
                    Amount = 200
                },
                new Price
                {
                    Id = Guid.Parse("2fa932a8-844a-4f83-a6c2-ed14b1e9aedc"),
                    Name = "Adult",
                    Amount = 300
                }
            );
        }
    }
}
