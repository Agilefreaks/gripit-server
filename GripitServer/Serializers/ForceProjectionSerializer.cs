using GripitServer.Models;

namespace GripitServer.Serializers
{
    public class ForceProjectionSerializer : IForceProjectionSerializer
    {
        private const string Separator = ",";

        public string Serialize(ForceProjecion forceProjecion)
        {
            return $"{forceProjecion.Id}{Separator}{forceProjecion.X}{Separator}{forceProjecion.Y}";
        }
    }
}