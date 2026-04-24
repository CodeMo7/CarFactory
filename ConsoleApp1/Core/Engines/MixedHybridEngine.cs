using CarFactory.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarFactory.Core.Engines
{
    public class MixedHybridEngine : IEngine
    {
        private GasolineEngine gasEngine = new GasolineEngine();
        private ElectronicEngine electricEngine = new ElectronicEngine();

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

            if (carSpeed < 50)
            {
                electricEngine.UpdateSpeed(carSpeed);
            }
            else
            {
                gasEngine.UpdateSpeed(carSpeed);
            }
        }
    }
}
