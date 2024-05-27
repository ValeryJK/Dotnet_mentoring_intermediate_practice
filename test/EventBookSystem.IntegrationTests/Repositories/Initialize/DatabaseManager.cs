using EventBookSystem.DAL.DataContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;

namespace EventBookSystem.IntegrationTests.Repositories.Initialize
{
    public class DatabaseManager
    {   
        private readonly RetryPolicy _retryPolicy;

        public DatabaseManager()
        {
            _retryPolicy = Policy
                .Handle<SqlException>(ex => ex.Number == 1205)
                .Or<DbUpdateConcurrencyException>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(5),
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

        public void ClearDatabase(MainDBContext context)
        {
            _retryPolicy.Execute(() =>
                {
                    context.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");

                    var tableNames = context.Model.GetEntityTypes().Select(t => t.GetTableName())
                        .Where(n => !string.IsNullOrEmpty(n)).ToList();

                    foreach (var tableName in tableNames)
                    {
                        var deleteCommand = $"EXEC('DELETE FROM [{tableName}]')";
                        context.Database.ExecuteSqlRaw(deleteCommand);
                    }

                    context.Database.ExecuteSqlRaw("EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'");

                    context.SaveChanges();

                    context.ChangeTracker.Clear();
                });
        }
    }
}