﻿using Quartz;
using Quartz.Impl;
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace wsSendingService
{
    public partial class Service1 : ServiceBase
    {
        private IScheduler scheduler;

        public Service1()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }
        protected override void OnStart(string[] args)
        {

            // Start the Quartz scheduler
            StartScheduler().GetAwaiter().GetResult();
        }

        protected override void OnStop()
        {
            // Shutdown the Quartz scheduler
            scheduler.Shutdown().GetAwaiter().GetResult();
        }

        private async Task StartScheduler()
        {
            int timerInterval = Convert.ToInt32(ConfigurationManager.AppSettings["TimerInterval"]);

            scheduler = await new StdSchedulerFactory().GetScheduler();

            // Start the scheduler
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<DataJob>()
                .WithIdentity("InsertDataJob", "Group1")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("InsertDataTrigger", "Group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(timerInterval)
                    .RepeatForever())
                .Build();

            // Schedule the job with the trigger
            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
