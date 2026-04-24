using CarFactory.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarFactory.Core.Engines
{
    public class ElectronicEngine : IEngine
    {
        public int Speed { get; private set; }

        public void Increase()
        {
            Speed++;
        }

        public void Decrease()
        {
            if (Speed > 0)
                Speed--;
        }

        public void UpdateSpeed(int carSpeed)
        {
            Speed = carSpeed;
        }
    }
}
