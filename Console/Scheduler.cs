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
                        _instance.Start();
                    }
                }
            }
 
            return _instance;
        }
    }
}