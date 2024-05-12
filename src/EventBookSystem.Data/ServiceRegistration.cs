using EventBookSystem.DAL.DataContext;
using EventBookSystem.DAL.Repositories;
using EventBookSystem.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventBookSystem.DAL
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddCoreDataServices(this IServiceCollection services, IConfiguration configuration, bool useInMemory)
        {
            if (useInMemory)
            {
                services.AddDbContext<MainDBContext>(opts => opts.UseInMemoryDatabase("EventBookSytemTestDB"));
            }
            else
            {
                services.AddDbContext<MainDBContext>(opts => opts.UseSqlServer(configuration.GetConnectionString("DbConnection")));
            }

            services.AddScoped<IRepositoryManager, RepositoryManager>();

            return services;
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
                o.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<MainDBContext>()
            .AddDefaultTokenProviders();
        }
    }
}
