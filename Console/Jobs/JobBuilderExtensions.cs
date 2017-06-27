using System;
using Quartz;

namespace Console.Jobs
{
    public static class JobBuilderExtensions
    {
        public const string RunUntil = "RunUntil";

        public static JobBuilder WithRunUntil(this JobBuilder jobBuilder, DateTime endDateTime)
        {
            jobBuilder.UsingJobData(RunUntil, endDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff"));;

            return jobBuilder;
        }
    }
}