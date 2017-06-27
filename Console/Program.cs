using System;
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
}