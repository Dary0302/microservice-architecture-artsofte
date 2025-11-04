using StackExchange.Redis;

namespace CoreLib.Sync;

public class RedisDistributedSemaphore : IDistributedSemaphore, IAsyncDisposable
{
    private readonly IDatabase db;
    private readonly string clientId;
    private readonly TimeSpan lockTtl;
    private readonly ConnectionMultiplexer connection;

    public RedisDistributedSemaphore(string connectionString, TimeSpan lockTtl)
    {
        connection = ConnectionMultiplexer.Connect(connectionString);
        db = connection.GetDatabase();
        clientId = Guid.NewGuid().ToString();
        this.lockTtl = lockTtl;
    }

    public async Task<bool> WaitAsync(string key, int maxCount, TimeSpan timeout)
    {
        var end = DateTime.UtcNow + timeout;

        while (DateTime.UtcNow < end)
        {
            var acquired = await TryAcquireAsync(key, maxCount);
            if (acquired)
                return true;

            await Task.Delay(100);
        }

        return false;
    }

    private async Task<bool> TryAcquireAsync(string key, int maxCount)
    {
        const string script = """
                              
                                          local holders = redis.call('lrange', KEYS[1], 0, -1)
                                          if #holders < tonumber(ARGV[1]) then
                                              redis.call('rpush', KEYS[1], ARGV[2])
                                              redis.call('pexpire', KEYS[1], ARGV[3])
                                              return 1
                                          end
                                          return 0
                                      
                              """;

        var result = (int)(long)(await db.ScriptEvaluateAsync(script,
            new RedisKey[] { key },
            new RedisValue[] { maxCount, clientId, (long)lockTtl.TotalMilliseconds }));

        return result == 1;
    }

    public async Task ReleaseAsync(string key)
    {
        const string script = """
                              
                                          local holders = redis.call('lrange', KEYS[1], 0, -1)
                                          for i, v in ipairs(holders) do
                                              if v == ARGV[1] then
                                                  redis.call('lrem', KEYS[1], 1, ARGV[1])
                                                  break
                                              end
                                          end
                                      
                              """;

        await db.ScriptEvaluateAsync(script, new RedisKey[] { key }, new RedisValue[] { clientId });
    }

    public async ValueTask DisposeAsync()
    {
        await connection.CloseAsync();
    }
}