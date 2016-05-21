using System;
using System.Reactive.Linq;
using System.Text;
using GripitServer.Models;
using SerialPortLib;

namespace GripitServer.Services
{
    public class DataPortal : IDataPortal
    {
        private readonly SerialPortInput _serialPort;
        private const string PortName = "/dev/ttyUSB0";
        private const int Baudrate = 115200;

        public DataPortal()
        {
            _serialPort = new SerialPortInput();
            _serialPort.SetPort(PortName, Baudrate);
            Messages = Observable.FromEvent<SerialPortInput.MessageReceivedEventHandler, MessageReceivedEventArgs>(
                h => _serialPort.MessageReceived += h,
                h => _serialPort.MessageReceived -= h)
                .Select(eventArgs => new DataFrame(Encoding.ASCII.GetString(eventArgs.Data)));
        }

        public IObservable<DataFrame> Messages { get; }

        public void Open()
        {
            _serialPort.Connect();
        }

        public void Close()
        {
            _serialPort.Disconnect();
        }
    }
}