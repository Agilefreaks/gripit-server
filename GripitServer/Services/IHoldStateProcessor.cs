using GripitServer.Models;

namespace GripitServer.Services
{
    public interface IHoldStateProcessor
    {
        ForceProjecion Process(ClimbingHoldState climbingHoldState);
    }
}