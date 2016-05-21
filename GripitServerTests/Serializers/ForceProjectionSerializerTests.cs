using FluentAssertions;
using GripitServer.Models;
using GripitServer.Serializers;
using NUnit.Framework;

namespace GripitServerTests.Serializers
{
    [TestFixture]
    public class ForceProjectionSerializerTests
    {
        private ForceProjectionSerializer _subject;

        [SetUp]
        public void SetUp()
        {
            _subject = new ForceProjectionSerializer();
        }

        [Test]
        public void Serialize_Always_ReturnsTheCorrectValue()
        {
            var forceProjecion = new ForceProjecion
            {
                Id = "A12",
                X = 2500,
                Y = 3000
            };

            var serializedValue = _subject.Serialize(forceProjecion);

            serializedValue.Should().Be("A12,2500,3000");
        }
    }
}