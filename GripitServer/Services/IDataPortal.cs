using System;
using GripitServer.Models;

namespace GripitServer.Services
{
    public interface IDataPortal : IStartable
    {
        IObservable<DataFrame> Messages { get; }
    }
}