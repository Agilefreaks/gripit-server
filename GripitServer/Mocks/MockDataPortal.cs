using System;
using System.Reactive.Linq;
using GripitServer.Models;
using GripitServer.Services;

namespace GripitServer.Mocks
{
    public class MockDataPortal : IDataPortal
    {
        private readonly Random _random;

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public MockDataPortal()
        {
            _random = new Random();
            Messages = Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(50))
                .Select(_ => GetRandomDataFrame());
        }

        public IObservable<DataFrame> Messages { get; }

        private DataFrame GetRandomDataFrame()
        {
            return new DataFrame(GetRandomStateString("X13") + GetRandomStateString("X14"));
        }

        private string GetRandomStateString(string id)
        {
            return $"{id}:{GetRandomValue():X},{GetRandomValue():X},{GetRandomValue():X},{GetRandomValue():X};";
        }

        private int GetRandomValue()
        {
            return _random.Next(ClimbingHoldState.MaxValue);
        }
    }
}