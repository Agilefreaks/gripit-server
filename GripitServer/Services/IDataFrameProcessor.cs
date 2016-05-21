using System.Collections.Generic;
using GripitServer.Models;

namespace GripitServer.Services
{
    public interface IDataFrameProcessor
    {
        IList<ClimbingHoldState> GetHoldStates(DataFrame getLastFrame);
    }
}