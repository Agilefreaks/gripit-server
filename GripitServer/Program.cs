using System;
using GripitServer.Repositories;
using GripitServer.Services;
using SimpleInjector;

namespace GripitServer
{
    internal class Program
    {
        private const string CloseMessage = "Press enter to stop recording data";

        private static Container _container;

        private static void Main()
        {
            SetupContainer();
            var gripitProcessor = _container.GetInstance<IGripitProcessor>();
            gripitProcessor.Start();

            Console.WriteLine(CloseMessage);
            Console.ReadLine();

            gripitProcessor.Stop();
        }

        private static void SetupContainer()
        {
            _container = new Container();
            _container.Register<IDataFrameProcessor, DataFrameProcessor>(Lifestyle.Singleton);
            _container.Register<IDataPortal, DataPortal>(Lifestyle.Singleton);
            _container.Register<IStateReaderService, StateReaderService>(Lifestyle.Singleton);
            _container.Register<IGripitProcessor, GripitProcessor>(Lifestyle.Singleton);
            _container.Register<ISchedulerProvider, SchedulerProvider>(Lifestyle.Singleton);
            _container.Register<IHoldStateProcessor, HoldStateProcessor>(Lifestyle.Singleton);
            _container.Register<IForceProjectionRepository, ForceProjectionRepository>(Lifestyle.Singleton);
            _container.Register<IForceProjectionSerializer, ForceProjectionSerializer>(Lifestyle.Singleton);
        }
    }
}
