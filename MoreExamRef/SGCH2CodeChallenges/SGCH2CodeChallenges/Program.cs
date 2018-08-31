using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCH2CodeChallenges
{
    class Program
    {
        static void Main(string[] args)
        {
            FahrenheitTemperature f = new FahrenheitTemperature(-30.0);
            Console.WriteLine("New Fahrenheit Object Instantiated. Temperature is: {0}", f.getTemperature());
            CelsiusTemperature c = f;
            Console.WriteLine("Converted to Celsius. Temperature is now: {0}", c.getTemperature());
            f = c;
            Console.WriteLine("Converted to back to Fahrenheit. Temperature is now: {0}", f.getTemperature());
        }
    }
}
