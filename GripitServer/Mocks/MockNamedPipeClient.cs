﻿using System;
using System.IO;
using System.IO.Pipes;
using System.Reactive.Linq;
using GripitServer.Services;

namespace GripitServer.Mocks
{
    public class MockNamedPipeClient : INamedPipeClient
    {
        private const string PipeName = "gripit";

        private readonly ISchedulerProvider _schedulerProvider;
        private StreamReader _reader;
        private IDisposable _dataSubscription;

        public MockNamedPipeClient(ISchedulerProvider schedulerProvider)
        {
            _schedulerProvider = schedulerProvider;
        }

        public void Start()
        {
            var client = new NamedPipeClientStream(PipeName);
            client.Connect();
            _reader = new StreamReader(client);

            _dataSubscription = Observable.Timer(TimeSpan.Zero, TimeSpan.FromMilliseconds(100))
                .SubscribeOn(_schedulerProvider.Default)
                .Select(_ => ReadData())
                .Do(Log)
                .Subscribe();
        }

        public void Stop()
        {
            _dataSubscription.Dispose();
        }

        private static void Log(string data)
        {
            Console.WriteLine(data);
        }


        private string ReadData()
        {
            return _reader.ReadLine();
        }
    }
}