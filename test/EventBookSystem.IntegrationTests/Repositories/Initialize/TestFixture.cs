using EventBookSystem.DAL.DataContext;
using Microsoft.Extensions.DependencyInjection;

namespace EventBookSystem.IntegrationTests.Repositories.Initialize
{
    public class TestFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; private set; }
        private readonly MainDBContext _context;

        private readonly DatabaseManager _databaseManager;
        private readonly DatabaseSeeder _databaseSeeder;

        public TestFixture()
        {
            ServiceProvider = TestServiceProvider.CreateServiceProvider();

            _context = GetDbContext();
            _databaseManager = new DatabaseManager(ServiceProvider);
            _databaseSeeder = new DatabaseSeeder(ServiceProvider);

            _databaseManager.EnsureDatabaseCreated(_context);
        }

        public MainDBContext GetDbContext()
        {
            var scope = ServiceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MainDBContext>();

            context.ChangeTracker.Clear();

            return context;
        }

        public void SeedDatabase(MainDBContext context)
        {
            _databaseSeeder.SeedDatabase(_context);
        }

        public void ClearAndSeed(MainDBContext context)
        {
            ClearDatabase(context);
            SeedDatabase(context);
        }

        public void ClearDatabase(MainDBContext context)
        {
            _databaseManager.ClearDatabase(_context)
                .GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            ClearDatabase(_context);
        }
    }
}