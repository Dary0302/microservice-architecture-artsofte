using System.Text.Json;
using CoreLib.Transport;

namespace DeliveryService.Application.Consumers;

public class AssignDeliveryConsumer : IRpcHandler
{
    public async Task<string> HandleAsync(string endpoint, string bodyJson)
    {
        if (endpoint == "assign")
        {
            var doc = JsonSerializer.Deserialize<JsonElement>(bodyJson);
            var result = new { success = true, assignedAt = DateTime.UtcNow };
            return JsonSerializer.Serialize(result);
        }

        return JsonSerializer.Serialize(new { error = "unknown endpoint" });
    }
}