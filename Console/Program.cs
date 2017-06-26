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
            
            var job = JobBuilder.Create<HelloWorldJob>()
                .WithIdentity("myJob", "group1")
                .UsingJobData(HelloWorldJob.IdentityId, 7)
                .Build();
		
            var trigger = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(5)
                    .WithRepeatCount(2))
                .Build();
		
            // Tell quartz to schedule the job using our trigger
            Scheduler.GetInstance().ScheduleJob(job, trigger);
            
            System.Console.WriteLine("Press any key to quit.");
            System.Console.ReadKey();
            
            Scheduler.GetInstance().Shutdown(true);
            System.Console.WriteLine("Scheduler Shutdown...");
        }
    }
}