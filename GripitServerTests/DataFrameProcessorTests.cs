using FluentAssertions;
using GripitServer.Models;
using GripitServer.Services;
using NUnit.Framework;

namespace GripitServerTests
{
    [TestFixture]
    public class DataFrameProcessorTests
    {
        private DataFrameProcessor _dataFrameProcessor;

        [SetUp]
        public void SetUp()
        {
            _dataFrameProcessor = new DataFrameProcessor();
        }

        [Test]
        public void GetHoldStates_DataStringIsEmpty_ReturnsEmptyList()
        {
            var lastFrame = new DataFrame(string.Empty);

            var states = _dataFrameProcessor.GetHoldStates(lastFrame);

            states.Should().BeEmpty();
        }

        [Test]
        public void GetHoldStates_DataStringDoesNotMatchPattern_ReturnsEmptyList()
        {
            var lastFrame = new DataFrame("random stuff");

            var states = _dataFrameProcessor.GetHoldStates(lastFrame);

            states.Should().BeEmpty();
        }

        [Test]
        public void GetHoldStates_DataStringMatchesPattern_ReturnsListOfStates()
        {
            var lastFrame = new DataFrame("X13:A23,BB,2C4,EEF;X14:A20,BC,2E6,FFF;");

            var states = _dataFrameProcessor.GetHoldStates(lastFrame);

            states.Count.Should().Be(2);
            states[0].Id.Should().Be("X13");
            states[0].UpValue.Should().Be(2595);
            states[0].RightValue.Should().Be(187);
            states[0].DownValue.Should().Be(708);
            states[0].LeftValue.Should().Be(3823);
            states[1].UpValue.Should().Be(2592);
            states[1].RightValue.Should().Be(188);
            states[1].DownValue.Should().Be(742);
            states[1].LeftValue.Should().Be(4095);
        }
    }
}