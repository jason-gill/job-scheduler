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

            System.Console.WriteLine($"{System.DateTime.Now:r}: Executing Job with IdentityId: {identityId}");

            _count++;
            // If job was successful
            if (_count == 2)
            {
                System.Console.WriteLine("Job executed successfully!");
                return true;
            }

            return false;
        }

        public override void LastExecutionWasUnsuccessfulCleanup(IJobExecutionContext context)
        {
            var data = context.JobDetail.JobDataMap;
            var identityId = data.GetIntValue(IdentityId);

            System.Console.WriteLine($"{System.DateTime.Now:r}: Cleaning up job with IdenittyId: {identityId}");
        }
    }
}