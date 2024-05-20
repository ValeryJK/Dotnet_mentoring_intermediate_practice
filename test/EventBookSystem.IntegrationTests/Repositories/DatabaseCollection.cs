using EventBookSystem.IntegrationTests.Repositories.Initialize;

namespace EventBookSystem.IntegrationTests.Repositories
{
    [CollectionDefinition("Database collection", DisableParallelization = true)]
    public class DatabaseCollection : ICollectionFixture<TestFixture> { }
}
