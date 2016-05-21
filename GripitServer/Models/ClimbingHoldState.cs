namespace GripitServer.Models
{
    public class ClimbingHoldState
    {
        public const int MaxValue = 1024;

        public string Id { get; set; }

        public int UpValue { get; set; }

        public int RightValue { get; set; }

        public int DownValue { get; set; }

        public int LeftValue { get; set; }
    }
}