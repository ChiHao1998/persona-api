using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace Configuration;

public static class HttpClientConfiguration
{
    public static IServiceCollection AddHttpClientWithPolicies<TInterface, TImplementation>(
        this IServiceCollection serviceList,
        HttpClientOption httpClientOption
        )
        where TInterface : class
        where TImplementation : class, TInterface
    {
        serviceList.AddHttpClient<TInterface, TImplementation>(client =>
        {
            client.BaseAddress = new Uri(httpClientOption.BaseUrl);

            if (httpClientOption.HeaderAcceptList != null)
                foreach (MediaTypeWithQualityHeaderValue mediaTypeWithQualityHeaderValue in httpClientOption.HeaderAcceptList)
                    client.DefaultRequestHeaders.Accept.Add(mediaTypeWithQualityHeaderValue);

            client.DefaultRequestHeaders.Authorization = httpClientOption.AuthenticationHeaderValue;

        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(httpClientOption.HandlerLifetimeMinute))
        .AddPolicyHandler(HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(result => result.StatusCode == HttpStatusCode.GatewayTimeout)
            .OrResult(result => result.StatusCode == HttpStatusCode.ServiceUnavailable)
            .OrResult(result => result.StatusCode == HttpStatusCode.BadGateway)
            .WaitAndRetryAsync(httpClientOption.RetryCount, retryAttempt =>
            {
                double exponential = Math.Pow(2, retryAttempt);
                double jitter = Random.Shared.NextDouble();

                return TimeSpan.FromSeconds(
                    httpClientOption.RetryInterval * exponential * jitter
                );
            })
        )
        .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(httpClientOption.TimeoutSeconds == 0 ?
        Timeout.InfiniteTimeSpan :
        TimeSpan.FromSeconds(httpClientOption.TimeoutSeconds)))
        .AddPolicyHandler(HttpPolicyExtensions
            .HandleTransientHttpError()
            .AdvancedCircuitBreakerAsync(
                failureThreshold: httpClientOption.FailureThresholdPercentage / 100,
                samplingDuration: TimeSpan.FromSeconds(httpClientOption.SamplingDurationSeconds),
                minimumThroughput: httpClientOption.MinimumThroughput,
                durationOfBreak: TimeSpan.FromSeconds(httpClientOption.BreakDurationSeconds)
        ));

        return serviceList;
    }
}
