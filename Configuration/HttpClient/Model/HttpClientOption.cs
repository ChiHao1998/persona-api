using System.Net.Http.Headers;
using Utility.Attribute;

namespace Configuration;

public class HttpClientOption
{
    [GenericRequired]
    public required string BaseUrl { get; set; }
    public AuthenticationHeaderValue? AuthenticationHeaderValue { get; set; }
    public HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue>? HeaderAcceptList { get; set; }
    public int BreakDurationSeconds { get; set; } = 30;
    public double FailureThresholdPercentage { get; set; } = 50;
    public int MinimumThroughput { get; set; } = 10;
    public int SamplingDurationSeconds { get; set; } = 10;
    public int TimeoutSeconds { get; set; } = 30;
    public int RetryInterval { get; set; } = 2;
    public int RetryCount { get; set; } = 3;
    public int HandlerLifetimeMinute { get; set; } = 5;
}