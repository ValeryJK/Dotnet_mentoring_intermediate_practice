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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Seat)
                .WithMany()
                .HasForeignKey(ci => ci.SeatId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Event>().Property(e => e.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Venue>().Property(v => v.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Section>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Seat>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Cart>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<CartItem>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Price>().Property(s => s.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Payment>().Property(s => s.Id).ValueGeneratedOnAdd();

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName() ?? string.Empty;
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            modelBuilder.ApplyConfiguration(new EventConfiguration());
            modelBuilder.ApplyConfiguration(new VenueConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new CartItemConfiguration());
            modelBuilder.ApplyConfiguration(new PriceConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new SeatConfiguration());
            modelBuilder.ApplyConfiguration(new SectionConfiguration());
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
