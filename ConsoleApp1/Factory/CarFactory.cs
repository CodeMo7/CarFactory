using CarFactory.Core.Engines;
using CarFactory.Core.Interfaces;
using CarFactory.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarFactory.Factory
{
    public static class CarFactory
    {
        public static Car CreateCar(EngineType engineType)
        {
            return new Car(CreateEngine(engineType));
        }

        public static void ChangeEngine(Car car, EngineType engineType)
        {
            car.SetEngine(CreateEngine(engineType));
        }

        private static IEngine CreateEngine(EngineType engineType)
        {
            return engineType switch
            {
                EngineType.Gasoline => new GasolineEngine(),
                EngineType.Electronic => new ElectronicEngine(),
                EngineType.Hybrid => new MixedHybridEngine(),
                _ => throw new ArgumentOutOfRangeException(nameof(engineType))
            };
        }
    }
}
