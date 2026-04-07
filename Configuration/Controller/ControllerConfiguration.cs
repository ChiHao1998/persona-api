using System.Text.Json;
using System.Text.Json.Serialization;
using Controller.Helper;
using Controller.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
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
            foreach (RateLimiterPolicy rateLimiterPolicy in rateLimiterPolicyList)
                options.AddFixedWindowLimiter(rateLimiterPolicy.Name, limiter =>
                {
                    limiter.PermitLimit = rateLimiterPolicy.RequestLimit;
                    limiter.Window = rateLimiterPolicy.Window;
                    limiter.QueueLimit = rateLimiterPolicy.QueueLimit;
                });
        });

        return services;
    }
}
