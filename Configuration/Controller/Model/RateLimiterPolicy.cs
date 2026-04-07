namespace Controller.Model
{
    public class RateLimiterPolicy
    {
        public required string Name { get; set; }
        public required int RequestLimit { get; set; }
        public required TimeSpan Window { get; set; }
        public int QueueLimit { get; set; } = 0;
    }
}