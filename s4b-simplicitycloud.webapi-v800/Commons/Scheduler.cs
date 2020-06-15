using FluentScheduler;
using SimplicityOnlineWebApi.Models.Interfaces;
using SimplicityOnlineWebApi.Models.Repositories;
using System;


namespace SimplicityOnlineWebApi.Commons
{

    public class RossumScheduler : Registry
    {
        // Classes written by Faheem Salik
        protected readonly IRossumRepository RossumRepository;
        public RossumScheduler(IRossumRepository rossumRepository)
        {
            this.RossumRepository = rossumRepository;
            NonReentrantAsDefault();
            // Schedule schedule = new Schedule(someMethod);
            // schedule.ToRunNow();
            Schedule(() => new RossumUpdatesJob(RossumRepository)).ToRunNow().AndEvery(120).Seconds();
        }

    }

    public class RossumUpdatesJob : IJob
    {
        protected readonly IRossumRepository RossumRepository;
        public RossumUpdatesJob(IRossumRepository rossumRepository)
        {
            RossumRepository = rossumRepository;
        }

        //public IDependencyObject DependencyObject { get; set; }

        public void Execute()
        {
            //this.RossumRepository.SchedulerRossumMainCall();
            //Console.WriteLine("Timed Task - Will run now");
        }
    }
}