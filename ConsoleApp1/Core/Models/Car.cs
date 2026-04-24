using CarFactory.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarFactory.Core.Models
{
    public class Car
    {
        private IEngine engine;

        public int Speed { get; private set; }

        public Car(IEngine engine)
        {
            this.engine = engine;
        }

        public void SetEngine(IEngine newEngine)
        {
            engine = newEngine;
        }

        public void Start()
        {
            Speed = 0;
            engine.UpdateSpeed(Speed);
            Console.WriteLine("Car started");
        }

        public void Stop()
        {
            Speed = 0;
            engine.UpdateSpeed(Speed);
            Console.WriteLine("Car stopped");
        }

        public void Accelerate()
        {
            if (Speed < 200)
            {
                Speed += 20;
                engine.UpdateSpeed(Speed);
            }

            Console.WriteLine($"Speed: {Speed}");
        }

        public void Brake()
        {
            if (Speed > 0)
            {
                Speed -= 20;
                if (Speed < 0) Speed = 0;

                engine.UpdateSpeed(Speed);
            }

            Console.WriteLine($"Speed: {Speed}");
        }
    }
}
