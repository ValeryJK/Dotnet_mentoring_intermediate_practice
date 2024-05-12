using EventBookSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventBookSystem.DAL.Configutations
{
    public class VenueConfiguration : IEntityTypeConfiguration<Venue>
    {
        public void Configure(EntityTypeBuilder<Venue> builder)
        {
            builder.HasData
            (
                new Venue
                {
                    Id = Guid.Parse("ec538ace-52ef-461a-ac55-8dce6345228d"),
                    DateUTC = DateTime.UtcNow,
                    Name = "Downtown Concert Hall",
                    Location = "New York City",
                },
                new Venue
                {
                    Id = Guid.Parse("7e4063df-1274-40ba-a839-decbcc8fd4fd"),
                    DateUTC = DateTime.UtcNow,
                    Name = "Grand Theater",
                    Location = "Chicago",
                }
            );
        }
    }
}
