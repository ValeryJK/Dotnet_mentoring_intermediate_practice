using EventBookSystem.Data.Entities;
using EventBookSystem.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventBookSystem.Data.Configutations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasData
            (
                new Payment
                {
                    Id = Guid.Parse("5dab4d39-bf3a-4f76-8786-c1933f343d42"),
                    PaymentMethod = "VISA",
                    Status = PaymentStatus.Unpaid,
                    Amount = 30,
                    DateUTC = DateTime.UtcNow,
                }
            );
        }
    }
}
