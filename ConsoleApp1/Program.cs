using System;
using CarFactory.Core.Models;
using CarFactory.Factory;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        { 
            var gasCar =CarFactory.Factory.CarFactory.CreateCar(EngineType.Gasoline);
            gasCar.Start();
            gasCar.Accelerate();
            gasCar.Accelerate();
            gasCar.Stop();

            Console.WriteLine("----");

            var electricCar = CarFactory.Factory.CarFactory.CreateCar(EngineType.Electronic);
            electricCar.Start();
            electricCar.Accelerate();
            electricCar.Brake();
            electricCar.Stop();

            Console.WriteLine("----");

            var hybridCar = CarFactory.Factory.CarFactory.CreateCar(EngineType.Hybrid);
            hybridCar.Start();

            hybridCar.Accelerate();
            hybridCar.Accelerate();
            hybridCar.Accelerate();

            hybridCar.Stop();

            Console.WriteLine("----");

            var speedTestCar = CarFactory.Factory.CarFactory.CreateCar(EngineType.Gasoline);
            speedTestCar.Start();

            for (int i = 0; i < 15; i++)
            {
                speedTestCar.Accelerate();
            }

            Console.WriteLine("Final Speed should not exceed 200");

            Console.WriteLine("----");

            var brakeTestCar = CarFactory.Factory.CarFactory.CreateCar(EngineType.Electronic);
            brakeTestCar.Start();
            brakeTestCar.Brake();
            brakeTestCar.Brake();

            Console.WriteLine("Speed should remain 0");

            Console.WriteLine("----");

            var engineSwitchCar = CarFactory.Factory.CarFactory.CreateCar(EngineType.Gasoline);
            engineSwitchCar.Start();
            engineSwitchCar.Accelerate();

            CarFactory.Factory.CarFactory.ChangeEngine(engineSwitchCar, EngineType.Hybrid);
            engineSwitchCar.Accelerate();

            CarFactory.Factory.CarFactory.ChangeEngine(engineSwitchCar, EngineType.Gasoline);
            engineSwitchCar.Accelerate();

            engineSwitchCar.Stop();
        }
    }
}