using System;
using GripitServer.Models;

namespace GripitServer.Services
{
    public interface IDataPortal
    {
        IObservable<DataFrame> Messages { get; }

        void Open();

        void Close();
    }
}