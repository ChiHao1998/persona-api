using Quartz;

namespace QuartzScheduler.Interface
{
    public interface IQuartzSchedulerBrokerService
    {
        ValueTask ScheduleJobAsync<TJob>(DateTimeOffset runAt, CancellationToken cancellationToken = default) where TJob : IJob;
    }
}