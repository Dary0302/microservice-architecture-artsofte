using System.Text.Json;
using CoreLib.Transport;

namespace DeliveryService.Infrastructure.Rabbit;

public class RabbitMqTransportServiceAdapter(string connectionString, string queueName = "rpc_queue")
    : ITransportService, IDisposable
{
    private readonly RabbitMqRpcClient client = new(connectionString);

    public async Task<TResponse?> RequestAsync<TRequest, TResponse>(string endpoint, TRequest request)
    {
        var wrapper = JsonSerializer.Serialize(new { endpoint, body = request });
        var raw = await client.CallAsync(queueName, wrapper, timeout: TimeSpan.FromSeconds(10));
        return JsonSerializer.Deserialize<TResponse>(raw);
    }

    public void Dispose() => client?.Dispose();
}