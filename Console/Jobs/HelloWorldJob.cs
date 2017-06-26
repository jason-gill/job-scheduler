using Quartz;

namespace Console.Jobs
{
    public class HelloWorldJob : IJob
    {
        public const string IdentityId = "IdentityId";

        private static int _count;
        private static bool _executedSuccessfully;
        
        public void Execute(IJobExecutionContext context)
        {
            var data = context.JobDetail.JobDataMap;
            var identityId = data.GetIntValue(IdentityId);
            var finalized = data.GetBooleanValue(SchedulerListener.Finalized);

            if (_executedSuccessfully)
            {
                System.Console.WriteLine($"{System.DateTime.Now:r}: Job with IdenittyId: {identityId} already executed successfully");
                
                // If the job executed successfully the last time we
                // don't need to run it again
                var e2 = new JobExecutionException {UnscheduleAllTriggers = true};
                throw e2;
            }
            
            if (finalized)
            {
                System.Console.WriteLine($"{System.DateTime.Now:r}: Finalizing job with IdenittyId: {identityId}");
                var e2 = new JobExecutionException {UnscheduleAllTriggers = true};
                throw e2;
            }
            
            System.Console.WriteLine($"{System.DateTime.Now:r}: Executing Job with IdentityId: {identityId}");

            _count++;
            if (_count == 99)
            {
                _executedSuccessfully = true;
                System.Console.WriteLine("Job executed successfully!");
                var e2 = new JobExecutionException {UnscheduleAllTriggers = true};
                throw e2;
            }
        }
    }
}