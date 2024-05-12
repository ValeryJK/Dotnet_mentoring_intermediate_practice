using EventBookSystem.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventBookSystem.Data.Configutations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            var hasher = new PasswordHasher<User>();

            var user1 = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "User",
                NormalizedUserName = "USER",
                Email = "test1@example.com",
                NormalizedEmail = "TEST1@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            user1.PasswordHash = hasher.HashPassword(user1, "user@123");

            var user2 = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "test2@example.com",
                NormalizedEmail = "TEST2@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };
            user2.PasswordHash = hasher.HashPassword(user2, "admin@123");

            builder.HasData(user1, user2);
        }
    }
}
