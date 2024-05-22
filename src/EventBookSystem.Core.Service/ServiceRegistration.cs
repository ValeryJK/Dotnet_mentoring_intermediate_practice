using EventBookSystem.Core.Service.MappingProfile;
using EventBookSystem.Core.Service.Services;
using EventBookSystem.Core.Service.Services.Interfaces;
using EventBookSystem.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventBookSystem.Core.Service
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {

            var useInMemory = configuration.GetValue<bool>("ConnectionStrings:UseInMemoryDatabase");

            services.AddCoreDataServices(configuration, useInMemory);
            services.ConfigureIdentity();

            services.AddAutoMapper(typeof(MappingCoreProfile));
            services.AddMemoryCache();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IVenueService, VenueService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}