using EventBookSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventBookSystem.DAL.Configutations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasData
            (
                new Event
                {
                    Id = Guid.Parse("d49dba7c-1c70-47a9-97d5-b595ec2f9173"),
                    Name = "Classical Music Evening",
                    DateUTC = DateTime.UtcNow,
                },
                new Event
                {
                    Id = Guid.Parse("1c756c0e-f149-41e7-acf8-3f34c98e5238"),
                    Name = "Opera Night",
                    DateUTC = DateTime.UtcNow,
                }
            );
        }
    }
}