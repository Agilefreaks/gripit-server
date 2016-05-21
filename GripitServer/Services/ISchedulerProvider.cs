using System.Reactive.Concurrency;

namespace GripitServer.Services
{
    public interface ISchedulerProvider
    {
        IScheduler Default { get; }
    }
}