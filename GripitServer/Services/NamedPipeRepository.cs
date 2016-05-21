using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace GripitServer.Services
{
    public abstract class NamedPipeRepository : INamedPipeRepository
    {
        private const string PipeName = "gripit";
        private Action<string> _logAction = NullLog;

        private readonly object _syncObject = new object();
        private StreamWriter _writer;

        protected NamedPipeRepository()
        {
            StartServer();
        }

        protected void Save<TEntity>(TEntity entity)
            where TEntity : class
        {
            _logAction(entity.ToString());
        }

        private void StartServer()
        {
            Task.Factory.StartNew(() =>
            {
                var server = new NamedPipeServerStream(PipeName);
                server.WaitForConnection();
                _writer = new StreamWriter(server);
                lock (_syncObject)
                {
                    _logAction = LogToPipe;
                }
            });
        }

        private static void NullLog(string message)
        {
        }

        private void LogToPipe(string message)
        {
            _writer.WriteLine(message);
        }
    }
}