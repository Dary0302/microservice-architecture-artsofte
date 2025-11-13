namespace CoreLib.Transport;

public interface IRpcHandler
{
    Task<string> HandleAsync(string endpoint, string bodyJson);
}