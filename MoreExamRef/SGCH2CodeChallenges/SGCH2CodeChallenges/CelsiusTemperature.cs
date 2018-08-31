using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCH2CodeChallenges
{
    class CelsiusTemperature
    {
        double temperature;

        public CelsiusTemperature(double temp)
        {
            this.temperature = temp;
        }

        public static implicit operator FahrenheitTemperature(CelsiusTemperature c)
        {
            return new FahrenheitTemperature((c.temperature) * (9 / (double)5) + 32);
        }

        public double getTemperature()
        {
            return temperature;
        }
    }
}
