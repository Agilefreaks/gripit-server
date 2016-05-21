using System;
using System.Reactive.Linq;
using GripitServer.Models;
using GripitServer.Serializers;

namespace GripitServer.Repositories
{
    public class ForceProjectionRepository : NamedPipeRepository, IForceProjectionRepository
    {
        private readonly IForceProjectionSerializer _serializer;

        public ForceProjectionRepository(IForceProjectionSerializer serializer)
        {
            _serializer = serializer;
        }

        public IObservable<ForceProjecion> Save(ForceProjecion state)
        {
            base.Save(_serializer.Serialize(state));
            return Observable.Return(state);
        }
    }
}