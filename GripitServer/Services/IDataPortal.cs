using GripitServer.Models;

namespace GripitServer.Services
{
    public interface IDataPortal
    {
        DataFrame GetLastFrame();
    }
}