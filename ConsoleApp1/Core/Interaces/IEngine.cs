using System;
using System.Collections.Generic;
using System.Text;

namespace CarFactory.Core.Interfaces
{
    public interface IEngine
    {
        int Speed { get; }

        void Increase();
        void Decrease();
        void UpdateSpeed(int carSpeed);
    }
}
