namespace CoreLib.Sync;

public interface IDistributedSemaphore
{
    Task<bool> WaitAsync(string key, int maxCount, TimeSpan timeout);
    Task ReleaseAsync(string key);
}
