using EventBookSystem.DAL.Configutations;
using EventBookSystem.DAL.Entities;
using EventBookSystem.Data.Configutations;
using EventBookSystem.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EventBookSystem.DAL.DataContext
{
    public class MainDBContext : IdentityDbContext<User>
    {
        public MainDBContext(DbContextOptions<MainDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Cart>()
               .Property(c => c.RowVersion)
               .IsRowVersion();

            builder.Entity<Payment>()
               .Property(c => c.RowVersion)
               .IsRowVersion();

            builder.Entity<Seat>()
              .Property(c => c.RowVersion)
              .IsRowVersion();

            builder.Entity<CartItem>()
             .Property(c => c.RowVersion)
             .IsRowVersion();

            builder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.Seat)
                .WithMany()
                .HasForeignKey(ci => ci.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Event>().Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Entity<Venue>().Property(v => v.Id).ValueGeneratedOnAdd();
            builder.Entity<Section>().Property(s => s.Id).ValueGeneratedOnAdd();
            builder.Entity<Seat>().Property(s => s.Id).ValueGeneratedOnAdd();
            builder.Entity<Cart>().Property(s => s.Id).ValueGeneratedOnAdd();
            builder.Entity<CartItem>().Property(s => s.Id).ValueGeneratedOnAdd();
            builder.Entity<Price>().Property(s => s.Id).ValueGeneratedOnAdd();
            builder.Entity<Payment>().Property(s => s.Id).ValueGeneratedOnAdd();

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName() ?? string.Empty;
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            builder.ApplyConfiguration(new EventConfiguration());
            builder.ApplyConfiguration(new VenueConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new CartConfiguration());
            builder.ApplyConfiguration(new CartItemConfiguration());
            builder.ApplyConfiguration(new PriceConfiguration());
            builder.ApplyConfiguration(new PaymentConfiguration());
            builder.ApplyConfiguration(new SeatConfiguration());
            builder.ApplyConfiguration(new SectionConfiguration());
        }

        public DbSet<Event> Events { get; set; } = null!;

        public DbSet<Venue> Venues { get; set; } = null!;

        public DbSet<Section> Sections { get; set; } = null!;

        public DbSet<Seat> Seats { get; set; } = null!;

        public DbSet<Cart> Carts { get; set; } = null!;

        public DbSet<CartItem> CartItems { get; set; } = null!;

        public DbSet<Price> Prices { get; set; } = null!;

        public DbSet<Payment> Payments { get; set; } = null!;
    }
}