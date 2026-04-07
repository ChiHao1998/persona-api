using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using SerilogLogger.Model;

namespace Serilog.Middleware
{
    public class HttpInboundLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpInboundLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            await _next(context);

            stopwatch.Stop();

            var requestHeaders = context.Request.Headers
                .ToDictionary(h => h.Key, h => (object)h.Value.ToString());

            var responseHeaders = context.Response.Headers
                .ToDictionary(h => h.Key, h => (object)h.Value.ToString());

            Log
            .ForContext<HttpInboundLog>()
            .ForContext("RequestId", context.TraceIdentifier)
            .ForContext("Method", context.Request.Method)
            .ForContext("Path", context.Request.Path.Value)
            .ForContext("QueryString", string.IsNullOrWhiteSpace(context.Request.QueryString.Value) ? null : context.Request.QueryString.Value)
            .ForContext("ClientIp", GetClientIp(context))
            .ForContext("UserId", string.IsNullOrWhiteSpace(context.User?.Identity?.Name) ? null : context.User?.Identity?.Name)
            .ForContext("StatusCode", context.Response.StatusCode)
            .ForContext("ElapsedMs", stopwatch.ElapsedMilliseconds)
            .ForContext("RequestHeaders", requestHeaders)
            .ForContext("ResponseHeaders", responseHeaders)
            .Information("HTTP Inbound Request");
        }

        private static string? GetClientIp(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out var ip))
                return ip.ToString();

            return context.Connection.RemoteIpAddress?.ToString();
        }
    }
}