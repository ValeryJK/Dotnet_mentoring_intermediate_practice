using EventBookSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventBookSystem.Data.Configutations
{
    public class SectionConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.HasData
            (
                new Section
                {
                    Id = Guid.Parse("58ce05b1-d34c-4a3e-a309-536d3b74be8b"),
                    Name = "A",
                    Capacity = 100,
                    DateUTC = DateTime.UtcNow,
                    VenueId = Guid.Parse("ec538ace-52ef-461a-ac55-8dce6345228d"),
                    EventId = Guid.Parse("d49dba7c-1c70-47a9-97d5-b595ec2f9173"),
                },
                new Section
                {
                    Id = Guid.Parse("3f14e23b-98e2-4646-8feb-180108ca17c5"),
                    Name = "B",
                    Capacity = 200,
                    DateUTC = DateTime.UtcNow,
                    VenueId = Guid.Parse("7e4063df-1274-40ba-a839-decbcc8fd4fd"),
                    EventId = Guid.Parse("1c756c0e-f149-41e7-acf8-3f34c98e5238"),
                }
            );
        }
    }
}
