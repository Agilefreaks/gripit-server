using System.Reactive.Concurrency;

namespace GripitServer.Services
{
    public class SchedulerProvider : ISchedulerProvider
    {
        public IScheduler Default => Scheduler.Default;
    }
}