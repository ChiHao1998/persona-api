using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using SerilogLogger.Model;

namespace Serilog.Middleware
{
    public class HttpInboundLoggingMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            await next(context);

            stopwatch.Stop();

            Dictionary<string, object> requestHeaderList = context.Request.Headers.ToDictionary(header => header.Key, header => (object)header.Value.ToString());

            Dictionary<string, object> responseHeaderList = context.Response.Headers.ToDictionary(header => header.Key, header => (object)header.Value.ToString());

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
            .ForContext("RequestHeaders", requestHeaderList)
            .ForContext("ResponseHeaders", responseHeaderList)
            .Information("HTTP Inbound Request");
        }

        private static string? GetClientIp(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue("X-Forwarded-For", out StringValues ip))
                return ip.ToString();

            return context.Connection.RemoteIpAddress?.ToString();
        }
    }
}