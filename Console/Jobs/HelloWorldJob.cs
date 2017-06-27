using Quartz;

namespace Console.Jobs
{
    public class HelloWorldJob : OnSuccessStopJob 
    {
        public const string IdentityId = "IdentityId";

        private static int _count;

        public override bool Run(IJobExecutionContext context)
        {
            var data = context.JobDetail.JobDataMap;
            var identityId = data.GetIntValue(IdentityId);

            System.Console.WriteLine($"{System.DateTime.Now:r}: Executing Job {context.JobDetail.Key} with IdentityId: {identityId}");

            _count++;
            // If job was successful
            if (_count == 4)
            {
                System.Console.WriteLine($"Job {context.JobDetail.Key} executed successfully!");
                return true;
            }

            return false;
        }

        public override void AfterAllAtemptsFailedCleanup(IJobExecutionContext context)
        {
            var data = context.JobDetail.JobDataMap;
            var identityId = data.GetIntValue(IdentityId);

            System.Console.WriteLine($"{System.DateTime.Now:r}: Cleaning up job {context.JobDetail.Key} with IdenittyId: {identityId}");
        }
    }
}