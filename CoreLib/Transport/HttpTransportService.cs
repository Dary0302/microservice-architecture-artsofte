using System.Net.Http.Json;
using System.Text.Json;

namespace CoreLib.Transport;

public class HttpTransportService(HttpClient client) : ITransportService
{
    private readonly JsonSerializerOptions jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<TResponse?> RequestAsync<TRequest, TResponse>(string endpoint, TRequest request)
    {
        var resp = await client.PostAsJsonAsync(endpoint, request);
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<TResponse>(jsonOptions);
    }
}