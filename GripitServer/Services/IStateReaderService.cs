using System;
using GripitServer.Models;

namespace GripitServer.Services
{
    public interface IStateReaderService
    {
        IObservable<ClimbingHoldState> ClimbingHoldStates { get; }
        void Start();
        void Stop();
    }
}