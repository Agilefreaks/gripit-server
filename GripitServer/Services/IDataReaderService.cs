using System;
using GripitServer.Models;

namespace GripitServer.Services
{
    public interface IDataReaderService
    {
        IObservable<ClimbingHoldState> ClimbingHoldStates { get; }
    }
}