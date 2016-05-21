using GripitServer.Models;

namespace GripitServer.Services
{
    public class DataPortal : IDataPortal
    {
        public DataFrame GetLastFrame()
        {
            return new DataFrame();
        }
    }
}