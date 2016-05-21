using System;
using GripitServer.Models;

namespace GripitServer.Repositories
{
    public interface IForceProjectionRepository
    {
        IObservable<ForceProjecion> Save(ForceProjecion state);
    }
}