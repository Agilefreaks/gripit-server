using FluentAssertions;
using GripitServer.Models;
using GripitServer.Services;
using NUnit.Framework;

namespace GripitServerTests.Services
{
    [TestFixture]
    public class HoldStateProcessorTests
    {
        private HoldStateProcessor _subject;

        [SetUp]
        public void SetUp()
        {
            _subject = new HoldStateProcessor();
        }

        [Test]
        public void Process_Always_ReturnsTheCorrectForceProjection()
        {
            var climbingHoldState = new ClimbingHoldState
            {
                UpValue = (int)(0.75 * ClimbingHoldState.MaxValue),
                RightValue = (int)(0.25 * ClimbingHoldState.MaxValue),
                DownValue = (int)(0.25 * ClimbingHoldState.MaxValue),
                LeftValue = (int)(0.75 * ClimbingHoldState.MaxValue)
            };

            var forceProjecion = _subject.Process(climbingHoldState);

            forceProjecion.X.Should().Be(-ForceProjecion.MaxValue / 2);
            forceProjecion.Y.Should().Be(ForceProjecion.MaxValue / 2);
        }
    }
}