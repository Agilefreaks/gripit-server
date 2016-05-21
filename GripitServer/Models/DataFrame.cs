namespace GripitServer.Models
{
    public class DataFrame
    {
        public DataFrame(string stringData)
        {
            DataString = stringData;
        }

        public string DataString { get; }
    }
}