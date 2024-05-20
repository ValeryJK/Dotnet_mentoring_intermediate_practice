using EventBookSystem.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventBookSystem.Data.Configutations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasData
            (
                new Cart
                {
                    Id = Guid.Parse("207b7a33-1503-4f50-86ea-b4ae6e3c1ed6"),
                    UUIDKey = Guid.Parse("8e834369-391e-4a8e-996f-141d9f19322f"),
                    DateUTC = DateTime.Now,
                }
            );
        }
    }
}