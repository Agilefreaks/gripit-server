using GripitServer.Models;

namespace GripitServer.Serializers
{
    public interface IForceProjectionSerializer
    {
        string Serialize(ForceProjecion forceProjecion);
    }
}