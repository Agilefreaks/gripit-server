using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using GripitServer.Models;

namespace GripitServer.Services
{
    public class DataReaderService : IDataReaderService
    {
        private const int SerialReadInterval = 100;
        private readonly Subject<ClimbingHoldState> _climbingHoldStateSubject;
        private readonly IDataPortal _dataPortal;
        private readonly IDataFrameProcessor _dataFrameProcessor;
        private readonly ISchedulerProvider _schedulerProvider;
        private IDisposable _readSubscription;

        public DataReaderService(
            IDataPortal dataPortal,
            IDataFrameProcessor dataFrameProcessor,
            ISchedulerProvider schedulerProvider)
        {
            _dataPortal = dataPortal;
            _dataFrameProcessor = dataFrameProcessor;
            _schedulerProvider = schedulerProvider;
            _climbingHoldStateSubject = new Subject<ClimbingHoldState>();
            ClimbingHoldStates = _climbingHoldStateSubject;
        }

        public IObservable<ClimbingHoldState> ClimbingHoldStates { get; }

        public void Start()
        {
            _readSubscription = Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(SerialReadInterval), _schedulerProvider.Default)
                .SelectMany(_ => _dataFrameProcessor.GetHoldStates(_dataPortal.GetLastFrame()))
                .Do(state => _climbingHoldStateSubject.OnNext(state))
                .ObserveOn(_schedulerProvider.Default)
                .SubscribeOn(_schedulerProvider.Default)
                .Subscribe();
        }

        public void Stop()
        {
            _readSubscription?.Dispose();
        }
    }
}