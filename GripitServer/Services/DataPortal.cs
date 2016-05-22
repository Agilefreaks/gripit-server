using System;
using System.Reactive.Subjects;
using System.Text;
using GripitServer.Models;
using SerialPortLib;

namespace GripitServer.Services
{
    public class DataPortal : IDataPortal
    {
        private const string PortName = "COM3";
        private const int Baudrate = 9600;

        private readonly SerialPortInput _serialPort;
        private readonly Subject<DataFrame> _dataFrameSubject;

        public DataPortal()
        {
            _serialPort = new SerialPortInput();
            _serialPort.SetPort(PortName, Baudrate);
            _serialPort.MessageReceived += SerialPortOnMessageReceived;
            _dataFrameSubject = new Subject<DataFrame>();
            Messages = _dataFrameSubject;
        }

        public IObservable<DataFrame> Messages { get; }

        public void Start()
        {
            _serialPort.Connect();
        }

        public void Stop()
        {
            _serialPort.Disconnect();
        }

        private void SerialPortOnMessageReceived(object sender, MessageReceivedEventArgs eventArgs)
        {
            _dataFrameSubject.OnNext(new DataFrame(Encoding.ASCII.GetString(eventArgs.Data)));
        }
    }
}