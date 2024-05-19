using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EventBookSystem.DAL;
using EventBookSystem.DAL.DataContext;
using Microsoft.EntityFrameworkCore;

namespace EventBookSystem.IntegrationTests.Repositories.Initialize
{
    public static class TestServiceProvider
    {
        public static IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            services.AddDbContext<MainDBContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("TestDbConnection"));
            });

            services.AddCoreDataServices(configuration, useInMemory: false);

            return services.BuildServiceProvider();
        }
    }
}