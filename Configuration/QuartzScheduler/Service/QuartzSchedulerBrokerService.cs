using Common.Interface;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Logging;
using QuartzScheduler.Interface;

namespace QuartzScheduler.Service
{
    public class QuartzSchedulerBrokerService(
        ISchedulerFactory iSchedulerFactory
    ) : IQuartzSchedulerBrokerService, IScopedService
    {
        public async ValueTask ScheduleJobAsync<TJob>(DateTimeOffset runAt, CancellationToken cancellationToken = default) where TJob : IJob
        {
            IScheduler scheduler = await iSchedulerFactory.GetScheduler(cancellationToken);

            JobKey jobKey = new(typeof(TJob).Name);
            TriggerKey triggerKey = new($"{typeof(TJob).Name}.trigger");

            if (await scheduler.CheckExists(jobKey, cancellationToken))
                await scheduler.DeleteJob(jobKey, cancellationToken);

            IJobDetail job = JobBuilder.Create<TJob>()
            .WithIdentity(jobKey)
            .Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity(triggerKey)
            .StartAt(runAt)
            .Build();

            await scheduler.ScheduleJob(job, trigger, cancellationToken);
        }
    }
}