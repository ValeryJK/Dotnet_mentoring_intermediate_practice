namespace EventBookSystem.Core.Service.Services.Interfaces
{
    public interface ILockManager
    {
        Task<IDisposable> AcquireLockAsync(Guid key);
    }
}