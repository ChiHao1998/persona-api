using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Api.Interface.Broker;
using Api.Model.Common;
using Common.Interface;
using Microsoft.Extensions.Options;
using Serilog;
using SerilogLogger.Model;

namespace Api.Service.Broker.VaultHttpClientBrokerService
{
    public partial class VaultHttpClientBrokerService(
        HttpClient httpClient,
        IOptionsMonitor<AppSettings> appSettings
    ) : IVaultHttpClientBrokerService, IScopedService
    {
        private readonly AppSettings appSettings = appSettings.CurrentValue;

        private async ValueTask<HttpResponseMessage> RequestAsync(HttpMethod method, string url, object? bodyContent = null, CancellationToken cancellationToken = default)
        {
            HttpRequestMessage httpRequestMessage = new(method: method, requestUri: url);

            if (bodyContent != null)
                httpRequestMessage.Content = new StringContent(content: JsonSerializer.Serialize(bodyContent), encoding: Encoding.UTF8, mediaType: "application/json");

            string requestId = new UUID().ToFormattedString();

            httpRequestMessage.Headers.Add("X-Request-Id", requestId);

            Stopwatch stopwatch = Stopwatch.StartNew();

            HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(request: httpRequestMessage, cancellationToken);

            stopwatch.Stop();

            Log.ForContext<HttpOutboundLog>()
            .ForContext("Method", method.Method)
            .ForContext("Host", httpRequestMessage.RequestUri?.Authority)
            .ForContext("Path", httpRequestMessage.RequestUri?.AbsolutePath)
            .ForContext("StatusCode", (int?)httpResponseMessage.StatusCode)
            .ForContext("ElapsedMs", stopwatch.ElapsedMilliseconds)
            .ForContext("RequestHeaders", httpRequestMessage.Headers.ToDictionary(h => h.Key, h => h.Value.ToArray()),
            destructureObjects: true)
            .ForContext("ResponseHeaders", httpResponseMessage.Headers.ToDictionary(h => h.Key, h => h.Value.ToArray()),
            destructureObjects: true)
            .ForContext("RequestId", requestId)
            .Information("HTTP outbound");

            return httpResponseMessage;
        }
    }
}
