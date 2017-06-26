using Quartz;
using Quartz.Impl;

namespace Console
{
    public sealed class Scheduler
    {
        private static IScheduler _instance;
 
        private static readonly object SyncLock = new object();
 
        public static IScheduler GetInstance()
        {
            if (_instance == null)
            {
                lock (SyncLock)
                {
                    if (_instance == null)
                    {
                        ISchedulerFactory sf = new StdSchedulerFactory();
                        _instance = sf.GetScheduler();
                        _instance.ListenerManager.AddSchedulerListener(new SchedulerListener());
                        _instance.Start();
                    }
                }
            }
 
            return _instance;
        }
    }

    public class SchedulerListener : ISchedulerListener
    {
        public const string Finalized = "Finalized";
        
        public void TriggerFinalized(ITrigger trigger)
        {
            var scheduler = Scheduler.GetInstance();
            
            var jobKey = trigger.JobKey;
            var currentJob = scheduler.GetJobDetail(jobKey);
            
            var jobDataMap = new JobDataMap(currentJob.JobDataMap.WrappedMap);
            jobDataMap.Put(Finalized, true);

            var finalJob = JobBuilder.Create(currentJob.JobType)
                .WithIdentity(jobKey.Name, "Finalized")
                .SetJobData(jobDataMap)
                .Build();
            
            var triggerOnce = TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithRepeatCount(0))
                .Build();

            System.Console.WriteLine($"TriggerFinalized - Scheduling Job: {jobKey.Name}");
            scheduler.ScheduleJob(finalJob, triggerOnce);
        }

        #region Not Implemented

        public void JobScheduled(ITrigger trigger)
        {
        }

        public void JobUnscheduled(TriggerKey triggerKey)
        {
        }

        public void TriggerPaused(TriggerKey triggerKey)
        {
        }

        public void TriggersPaused(string triggerGroup)
        {
        }

        public void TriggerResumed(TriggerKey triggerKey)
        {
        }

        public void TriggersResumed(string triggerGroup)
        {
        }

        public void JobAdded(IJobDetail jobDetail)
        {
        }

        public void JobDeleted(JobKey jobKey)
        {
        }

        public void JobPaused(JobKey jobKey)
        {
        }

        public void JobsPaused(string jobGroup)
        {
        }

        public void JobResumed(JobKey jobKey)
        {
        }

        public void JobsResumed(string jobGroup)
        {
        }

        public void SchedulerError(string msg, SchedulerException cause)
        {
        }

        public void SchedulerInStandbyMode()
        {
        }

        public void SchedulerStarted()
        {
        }

        public void SchedulerStarting()
        {
        }

        public void SchedulerShutdown()
        {
        }

        public void SchedulerShuttingdown()
        {
        }

        public void SchedulingDataCleared()
        {
        }
        
        #endregion
    }
}