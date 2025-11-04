namespace CoreLib.Transport;

public interface ITransportService
{
    Task<TResponse?> RequestAsync<TRequest, TResponse>(string endpoint, TRequest request);
}