using Quartz;

namespace Console.Jobs
{
    public class HelloWorldJob : IJob
    {
        public const string IdentityId = "IdentityId";

        private static int _count;
        
        public void Execute(IJobExecutionContext context)
        {
            var isLastExecution = !context.NextFireTimeUtc.HasValue;
            var data = context.JobDetail.JobDataMap;
            var identityId = data.GetIntValue(IdentityId);

            System.Console.WriteLine($"{System.DateTime.Now:r}: Executing Job with IdentityId: {identityId}");

            _count++;
            // If job was successful
            if (_count == 2)
            {
                System.Console.WriteLine("Job executed successfully!");
                context.Scheduler.DeleteJob(context.JobDetail.Key);
//                var stopExecuting = new JobExecutionException {UnscheduleAllTriggers = true};
//                throw stopExecuting;
            }

            if (isLastExecution)
            {
                System.Console.WriteLine($"{System.DateTime.Now:r}: Finalizing job with IdenittyId: {identityId}");
            }
        }
    }
}