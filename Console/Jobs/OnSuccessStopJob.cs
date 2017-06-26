using Quartz;

namespace Console.Jobs
{
    public abstract class OnSuccessStopJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var executionWasSuccessful = Run(context);
            if (executionWasSuccessful)
            {
                context.Scheduler.DeleteJob(context.JobDetail.Key);
            }

            var isLastExecution = !context.NextFireTimeUtc.HasValue;
            if (isLastExecution && !executionWasSuccessful)
            {
                LastExecutionWasUnsuccessfulCleanup(context);
            }
        }

        public abstract bool Run(IJobExecutionContext context);

        public virtual void LastExecutionWasUnsuccessfulCleanup(IJobExecutionContext context)
        {
        }
    }
}