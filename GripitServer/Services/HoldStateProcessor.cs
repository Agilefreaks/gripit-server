using GripitServer.Models;

namespace GripitServer.Services
{
    public class HoldStateProcessor : IHoldStateProcessor
    {
        private const int HorizontalOffset = 0;
        private const int VerticalOffset = 0;
        private const int RightWeight = 1;
        private const int LeftWeight = 1;
        private const int UpWeight = 1;
        private const int DownWeight = 1;

        public ForceProjecion Process(ClimbingHoldState climbingHoldState)
        {
            var horizontalOffset = HorizontalOffset +
                                   (climbingHoldState.RightValue*RightWeight - climbingHoldState.LeftValue*LeftWeight);
            var verticalOffset = VerticalOffset +
                                 (climbingHoldState.UpValue*UpWeight - climbingHoldState.DownValue*DownWeight);

            var forceProjecion = new ForceProjecion
            {
                Id = climbingHoldState.Id,
                X = Project(horizontalOffset, ClimbingHoldState.MaxValue, ForceProjecion.MaxValue),
                Y = Project(verticalOffset, ClimbingHoldState.MaxValue, ForceProjecion.MaxValue)
            };

            return forceProjecion;
        }

        private static int Project(int offset, int sourceMaxValue, int targetMaxValue)
        {
            return (int)((decimal)offset/sourceMaxValue*targetMaxValue);
        }
    }
}