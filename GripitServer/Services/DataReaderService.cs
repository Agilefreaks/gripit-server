using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using GripitServer.Models;

namespace GripitServer.Services
{
    public class DataReaderService : IDataReaderService
    {
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
            _readSubscription = _dataPortal.Messages
                .SelectMany(dataFrame => _dataFrameProcessor.GetHoldStates(dataFrame))
                .Do(state => _climbingHoldStateSubject.OnNext(state))
                .ObserveOn(_schedulerProvider.Default)
                .SubscribeOn(_schedulerProvider.Default)
                .Subscribe();
            _dataPortal.Open();
        }

        public void Stop()
        {
            _readSubscription?.Dispose();
            _dataPortal.Close();
        }
    }
}