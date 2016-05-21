using System;
using System.Reactive.Linq;
using GripitServer.Repositories;

namespace GripitServer.Services
{
    public class GripitProcessor : IGripitProcessor
    {
        private readonly IStateReaderService _stateReaderService;
        private readonly IHoldStateProcessor _holdStateProcessor;
        private readonly IForceProjectionRepository _forceProjectionRepository;
        private readonly ISchedulerProvider _schedulerProvider;
        private IDisposable _stateSubscription;

        public GripitProcessor(
            IStateReaderService stateReaderService,
            IHoldStateProcessor holdStateProcessor,
            IForceProjectionRepository forceProjectionRepository,
            ISchedulerProvider schedulerProvider)
        {
            _stateReaderService = stateReaderService;
            _holdStateProcessor = holdStateProcessor;
            _forceProjectionRepository = forceProjectionRepository;
            _schedulerProvider = schedulerProvider;
        }

        public void Start()
        {
            _stateReaderService.Start();
            _stateSubscription = _stateReaderService.ClimbingHoldStates
                .Select(state => _holdStateProcessor.Process(state))
                .Select(forceProjection => _forceProjectionRepository.Save(forceProjection))
                .Switch()
                .SubscribeOn(_schedulerProvider.Default)
                .Subscribe();
        }

        public void Stop()
        {
            _stateReaderService.Stop();
            _stateSubscription?.Dispose();
        }
    }
}