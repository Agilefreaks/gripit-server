using System.Reactive.Linq;
using GripitServer.Models;
using GripitServer.Services;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;

namespace GripitServerTests
{
    [TestFixture]
    public class GripitProcessorTests
    {
        private GripitProcessor _subject;
        private Mock<IStateReaderService> _mockStateReaderService;
        private Mock<IHoldStateProcessor> _mockStateProcessor;
        private Mock<IForceProjectionRepository> _mockForceProjectionRepository;
        private Mock<ISchedulerProvider> _mockSchedulerProvider;
        private TestScheduler _testScheduler;

        [SetUp]
        public void SetUp()
        {
            _mockStateReaderService = new Mock<IStateReaderService>();
            _mockStateProcessor = new Mock<IHoldStateProcessor>();
            _mockForceProjectionRepository = new Mock<IForceProjectionRepository>();
            _mockSchedulerProvider = new Mock<ISchedulerProvider>();
            _subject = new GripitProcessor(
                _mockStateReaderService.Object,
                _mockStateProcessor.Object, 
                _mockForceProjectionRepository.Object,
                _mockSchedulerProvider.Object);
            _mockStateReaderService.Setup(sr => sr.ClimbingHoldStates)
                .Returns(Observable.Never<ClimbingHoldState>());
            _mockForceProjectionRepository.Setup(r => r.Save(It.IsAny<ForceProjecion>()))
                .Returns(Observable.Never<ForceProjecion>());
            _testScheduler = new TestScheduler();
            _mockSchedulerProvider.SetupGet(p => p.Default).Returns(_testScheduler);
        }

        [Test]
        public void Start_Always_StartTheDataReader()
        {
            _subject.Start();

            _mockStateReaderService.Verify(sr => sr.Start());
        }

        [Test]
        public void Start_AStateIsReceived_SavesTheProcessedForceProjection()
        {
            var newClimbingHoldState = new ClimbingHoldState();
            _mockStateReaderService.Setup(sr => sr.ClimbingHoldStates)
                .Returns(Observable.Return(newClimbingHoldState));
            var forceProjecion = new ForceProjecion();
            _mockStateProcessor.Setup(p => p.Process(newClimbingHoldState)).Returns(forceProjecion);

            _subject.Start();
            _testScheduler.Start();

            _mockForceProjectionRepository.Verify(r => r.Save(forceProjecion));
        }

        [Test]
        public void Stop_Always_StopsTheDataReader()
        {
            _subject.Stop();

            _mockStateReaderService.Verify(sr => sr.Stop());
        }
    }
}