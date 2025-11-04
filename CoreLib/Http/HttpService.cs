using System.Net.Http.Json;

namespace CoreLib.Http
{
    public class HttpService
    {
        private readonly HttpClient client;

        public HttpService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<T?> GetAsync<T>(string url)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest body)
        {
            var response = await client.PostAsJsonAsync(url, body);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }
    }
}