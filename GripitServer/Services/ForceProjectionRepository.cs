﻿using System;
using System.Reactive.Linq;
using GripitServer.Models;

namespace GripitServer.Services
{
    public class ForceProjectionRepository : NamedPipeRepository, IForceProjectionRepository
    {
        public IObservable<ForceProjecion> Save(ForceProjecion state)
        {
            base.Save(state);
            return Observable.Return(state);
        }
    }
}