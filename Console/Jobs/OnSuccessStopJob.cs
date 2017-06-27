using System;
using Quartz;

namespace Console.Jobs
{
    public abstract class OnSuccessStopJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            DateTime runUntil;

            var executionWasSuccessful = Run(context);
            if (executionWasSuccessful)
            {
                context.Scheduler.DeleteJob(context.JobDetail.Key);
                return;
            }

            var hasRunUntil = DateTime.TryParse(dataMap.GetString(JobBuilderExtensions.RunUntil), out runUntil);
            if (hasRunUntil && (runUntil < DateTime.Now))
            {
                AfterAllAtemptsFailedCleanup(context);
                context.Scheduler.DeleteJob(context.JobDetail.Key);
                return;
            }

            var isLastExecution = !context.NextFireTimeUtc.HasValue;
            if (isLastExecution)
            {
                AfterAllAtemptsFailedCleanup(context);
            }
        }

        public abstract bool Run(IJobExecutionContext context);

        public virtual void AfterAllAtemptsFailedCleanup(IJobExecutionContext context)
        {
        }
    }
}