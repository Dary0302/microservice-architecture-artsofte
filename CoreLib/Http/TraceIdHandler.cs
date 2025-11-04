namespace CoreLib.Http
{
    public class TraceIdHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Trace-Id", TraceIdProvider.TraceId);
            return base.SendAsync(request, cancellationToken);
        }
    }
}