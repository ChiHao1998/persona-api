using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Common.Model;
using Common.Model.CustomEnum;
using Controller.Helper;
using Controller.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Configuration;

public static class ControllerConfiguration
{
    public static IServiceCollection SetupController(this IServiceCollection services, string applicationName)
    {
        services.AddControllers()
        .AddMvcOptions(options =>
        {
            options.UseGeneralRoutePrefix($"api/{applicationName}");

            options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
            options.Filters.Add(new ProducesAttribute("application/json"));
        })
        .AddJsonOptions(options =>
        {
            JsonSerializerOptions json = options.JsonSerializerOptions;

            json.Converters.Add(new JsonConverterHelper());
            json.PropertyNameCaseInsensitive = true;
            json.PropertyNamingPolicy = null;

            json.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            json.NumberHandling = JsonNumberHandling.AllowReadingFromString;
        });

        services.Configure<RouteOptions>(options =>
        {
            options.LowercaseUrls = true;
            options.LowercaseQueryStrings = true;
            options.AppendTrailingSlash = false;
        });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions["traceId"] =
                    context.HttpContext.TraceIdentifier;
            };
        });

        services.AddEndpointsApiExplorer();

        return services;
    }

    public static IServiceCollection SetupRequestTimeout(this IServiceCollection services, TimeSpan timeSpan)
    {
        services.AddRequestTimeouts(options =>
        {
            options.DefaultPolicy =
                new RequestTimeoutPolicy
                {
                    Timeout = timeSpan
                };
        });

        return services;
    }

    public static IServiceCollection SetupRateLimiter(this IServiceCollection services, List<RateLimiterPolicy> rateLimiterPolicyList)
    {
        services.AddRateLimiter(options =>
        {
            // =========================================
            // IP limiters
            // =========================================
            foreach (RateLimiterPolicy rateLimiterPolicy in rateLimiterPolicyList)
            {
                options.AddPolicy(rateLimiterPolicy.Name, httpContext =>
                {
                    httpContext.Items["RateLimitType"] = rateLimiterPolicy.Type.ToString();

                    string key = rateLimiterPolicy.Type switch
                    {
                        RateLimitTypeEnum.Ip => httpContext.Connection.RemoteIpAddress?.ToString() ?? "N/A",

                        _ => "N/A"
                    };

                    return RateLimitPartition.GetTokenBucketLimiter(
                        partitionKey: key,
                        factory: _ => new TokenBucketRateLimiterOptions
                        {
                            TokenLimit = rateLimiterPolicy.RequestLimit,
                            TokensPerPeriod = 1,
                            ReplenishmentPeriod = rateLimiterPolicy.Window,
                            AutoReplenishment = true,
                            QueueLimit = rateLimiterPolicy.QueueLimit,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        });
                });
            }

            // =========================================
            // Common rejection handler
            // =========================================
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.OnRejected = async (context, token) =>
            {
                HttpContext httpContext = context.HttpContext;

                string rateLimitType = httpContext.Items["RateLimitType"] as string ?? "N/A";

                string message = rateLimitType switch
                {
                    RateLimitTypeEnum.Ip => "Too many requests. Please slow down.",

                    _ => "Rate limit exceeded."
                };

                httpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.Headers["X-RateLimit-Type"] = rateLimitType;

                ResponseWrapper response = ResponseBuilder
                    .CreateFail(new Error(
                        ErrorCodeEnum.RATE_LIMIT_EXCEEDED,
                        message))
                    .Build();

                await httpContext.Response.WriteAsJsonAsync(response, cancellationToken: token);
            };
        });

        return services;
    }
}
