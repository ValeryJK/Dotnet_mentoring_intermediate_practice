using EventBookSystem.Core.Service.MappingProfile;
using EventBookSystem.Core.Service.Services;
using EventBookSystem.Core.Service.Services.Interfaces;
using EventBookSystem.DAL;
using EventBookSystem.DAL.Repositories;
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

            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped<IServiceManager, ServiceManager>();

            return services;
        }
    }
}