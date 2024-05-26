using EventBookSystem.Core.Service.Services.Interfaces;
using System.Collections.Concurrent;

namespace EventBookSystem.Core.Service.Services
{
    public class LockManager : ILockManager
    {
        private readonly ConcurrentDictionary<Guid, SemaphoreSlim> _locks = new ConcurrentDictionary<Guid, SemaphoreSlim>();

        public async Task<IDisposable> AcquireLockAsync(Guid key)
        {
            var semaphore = _locks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
            await semaphore.WaitAsync();
            return new SemaphoreSlimReleaser(() => ReleaseLock(key, semaphore));
        }

        private void ReleaseLock(Guid key, SemaphoreSlim semaphore)
        {
            semaphore.Release();
            if (semaphore.CurrentCount == 1)
            {
                _locks.TryRemove(key, out _);
            }
        }
    }

    public class SemaphoreSlimReleaser : IDisposable
    {
        private readonly Action _release;
        private bool _disposed;

        public SemaphoreSlimReleaser(Action release)
        {
            _release = release;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {                    
                    _release();
                }
               
                _disposed = true;
            }
        }
    }
}