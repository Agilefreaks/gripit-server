using System;
using System.Collections.Generic;
using System.Reactive;
using FluentAssertions;
using GripitServer.Models;
using GripitServer.Services;
using Microsoft.Reactive.Testing;
using Moq;
using NUnit.Framework;

namespace GripitServerTests
{
    [TestFixture]
    public class DataReaderServiceTests
    {
        private DataReaderService _subject;
        private Mock<IDataPortal> _mockDataPortal;
        private Mock<IDataFrameProcessor> _mockDataFrameProcessor;
        private Mock<ISchedulerProvider> _mockSchedulerProvider;
        private TestScheduler _testScheduler;

        [SetUp]
        public void SetUp()
        {
            _mockDataPortal = new Mock<IDataPortal>();
            _mockDataFrameProcessor = new Mock<IDataFrameProcessor>();
            _mockSchedulerProvider = new Mock<ISchedulerProvider>();
            _subject = new DataReaderService(
                _mockDataPortal.Object,
                _mockDataFrameProcessor.Object,
                _mockSchedulerProvider.Object);
            _testScheduler = new TestScheduler();
            _mockSchedulerProvider.SetupGet(s => s.Default).Returns(_testScheduler);
        }

        [TearDown]
        public void TearDown()
        {
            _subject.Stop();
        }

        [Test]
        public void Start_AfterReceivingADataFrameFromTheDataPortal_EmitsTheContainedStates()
        {
            var dataFrame = new DataFrame("someFrame");
            var observable = _testScheduler.CreateColdObservable(
                new Recorded<Notification<DataFrame>>(10, Notification.CreateOnNext(dataFrame)),
                new Recorded<Notification<DataFrame>>(20, Notification.CreateOnCompleted<DataFrame>()));
            _mockDataPortal.Setup(d => d.Messages).Returns(observable);
            var climbingHoldStates = new List<ClimbingHoldState>
            {
                new ClimbingHoldState { Id = "A12", DownValue = 12, LeftValue = 13 },
                new ClimbingHoldState { Id = "A13", DownValue = 14, LeftValue = 15 }
            };
            _mockDataFrameProcessor.Setup(p => p.GetHoldStates(dataFrame)).Returns(climbingHoldStates);
            var emittedStates = new List<ClimbingHoldState>();
            var stateSubscription = _subject.ClimbingHoldStates.Subscribe(state => emittedStates.Add(state));

            _subject.Start();
            _testScheduler.AdvanceBy(50);

            stateSubscription.Dispose();
            emittedStates.Count.Should().Be(2);
            emittedStates[0].Id.Should().Be("A12");
            emittedStates[1].Id.Should().Be("A13");
        }
    }
}