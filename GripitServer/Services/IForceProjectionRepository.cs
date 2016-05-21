using System;
using GripitServer.Models;

namespace GripitServer.Services
{
    public interface IForceProjectionRepository
    {
        IObservable<ForceProjecion> Save(ForceProjecion state);
    }
}