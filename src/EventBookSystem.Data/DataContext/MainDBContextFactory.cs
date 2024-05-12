using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace EventBookSystem.DAL.DataContext
{
    public class MainDBContextFactory : IDesignTimeDbContextFactory<MainDBContext>
    {
        public MainDBContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var builder = new DbContextOptionsBuilder<MainDBContext>();
            var connectionString = configuration.GetConnectionString("DbConnection");
            builder.UseSqlServer(connectionString);

            return new MainDBContext(builder.Options);
        }
    }

}
