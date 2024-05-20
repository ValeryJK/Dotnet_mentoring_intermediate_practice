using EventBookSystem.DAL.DataContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;

namespace EventBookSystem.IntegrationTests.Repositories.Initialize
{
    public class DatabaseManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AsyncRetryPolicy _retryPolicy;

        public DatabaseManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            _retryPolicy = Policy
                .Handle<SqlException>(ex => ex.Number == 1205)
                .Or<DbUpdateConcurrencyException>()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(5),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        Console.WriteLine($"Retry {retryCount} due to {exception.GetType().Name}: {exception.Message}");
                    });
        }

        public void EnsureDatabaseCreated(MainDBContext context)
        {
            context.Database.EnsureCreated();
            context.Database.Migrate();
        }

        public async Task ClearDatabase(MainDBContext context)
        {
            await _retryPolicy.ExecuteAsync(async () =>
                {
                    context.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");

                    var tableNames = context.Model.GetEntityTypes().Select(t => t.GetTableName())
                        .Where(n => !string.IsNullOrEmpty(n)).ToList();

                    tableNames.ForEach(tableName => context.Database.ExecuteSqlRaw($"DELETE FROM [{tableName}]"));
                    await context.Database.ExecuteSqlRawAsync("EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'");

                    await context.SaveChangesAsync();

                    context.ChangeTracker.Clear();
                });
        }
    }
}