using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CoreLib.Http
{
    public class TraceIdMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("Trace-Id", out var traceId))
                TraceIdProvider.TraceId = traceId!;
            else
                TraceIdProvider.TraceId = Guid.NewGuid().ToString();

            context.Response.Headers["Trace-Id"] = TraceIdProvider.TraceId;

            await next(context);
        }
    }

    public static class TraceIdMiddlewareExtensions
    {
        public static IApplicationBuilder UseTraceId(this IApplicationBuilder app) =>
            app.UseMiddleware<TraceIdMiddleware>();
    }
}