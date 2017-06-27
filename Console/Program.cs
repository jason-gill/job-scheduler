using System;
using System.Runtime.CompilerServices;
using Console.Jobs;
using Quartz;

namespace Console
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            System.Console.WriteLine("Starting Scheduler...");
           
            Scheduler.GetInstance();

            var endDateTime = DateTime.Now.AddMinutes(2);

            var retryableJob = RetryableJobBuilder.Create<HelloWorldJob>(endDateTime, 15)
                .AddJobData("", "")
                .AddJobData("", "")
                .Build();
            Scheduler.GetInstance().ScheduleJob(retryableJob.JobDetail, retryableJob.Trigger);
            
            var job = JobBuilder.Create<HelloWorldJob>()
                .WithIdentity(Guid.NewGuid().ToString(), "Retry")
                .WithRunUntil(endDateTime)
                .UsingJobData(HelloWorldJob.IdentityId, 7)
                .Build();

            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(15)
                    .WithRepeatCount(8))
                .Build();
		
            // Tell quartz to schedule the job using our trigger
            Scheduler.GetInstance().ScheduleJob(job, trigger);
            
            System.Console.WriteLine("Press any key to shutdown the scheduler.");
            System.Console.ReadKey();
            Scheduler.GetInstance().Shutdown(true);

            System.Console.WriteLine("Press any key to start a new scheduler.");
            System.Console.ReadKey();
            Scheduler.GetInstance(true);
            
            System.Console.WriteLine("Press any key to quit");
            System.Console.ReadKey();
            Scheduler.GetInstance().Shutdown(true);
            System.Console.WriteLine("Goodbye!");
        }
    }
    
    public class RetryableJobBuilder
    {

        private JobDataMap jobDataMap = new JobDataMap();

        // What we need
        // =====================
        // OnSuccessStopJob Type
        // Any job related data (e.g. database id's, other meta data)
        // Ending Date
        // Interval to run

        public IJobDetail JobDetail { get; private set; }
        public ITrigger Trigger { get; private set; }

        protected RetryableJobBuilder(DateTime runUntil, int runInterval)
        {

        }

        public static RetryableJobBuilder Create<T>(DateTime runUntil, int runInterval) where T : OnSuccessStopJob 
        {
            var retryJobBuilder = new RetryableJobBuilder(runUntil, runInterval);
            return retryJobBuilder;
        }

        public void Build()
        {
            
        }
    }

    public class RetryableJob
    {
        
    }
}