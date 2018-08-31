using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCH2CodeChallenges
{
    class FahrenheitTemperature
    {
        double temperature;

        public FahrenheitTemperature(double temp)
        {
            this.temperature = temp;
        }

        public static implicit operator CelsiusTemperature(FahrenheitTemperature f)
        {
            return new CelsiusTemperature((f.temperature - 32) * (5 / (double)9));
        }

        public double getTemperature()
        {
            return temperature;
        }
    }
}