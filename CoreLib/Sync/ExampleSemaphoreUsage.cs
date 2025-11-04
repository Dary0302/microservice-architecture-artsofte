using CoreLib.Sync;

public class ExampleSemaphoreUsage
{
    public static async Task Run()
    {
        var semaphore = new RedisDistributedSemaphore("localhost:6379", TimeSpan.FromSeconds(30));

        if (await semaphore.WaitAsync("video:process", 3, TimeSpan.FromSeconds(10)))
        {
            try
            {
                Console.WriteLine("Semaphore acquired, doing work...");
                await Task.Delay(5000);
            }
            finally
            {
                await semaphore.ReleaseAsync("video:process");
                Console.WriteLine("Semaphore released.");
            }
        }
        else
        {
            Console.WriteLine("Failed to acquire semaphore.");
        }
    }
}