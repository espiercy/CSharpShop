using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCH3CodeChallenges
{
    abstract class Transformer
    {
        protected int Wheels { get; set; }
        protected int Speed { get; set; }

        public virtual void Run()
        {
            Console.WriteLine("Transformer is now a: {0} with {1} wheels and a maximum speed of {2}", this.GetType(), Wheels, Speed);
        }
    }
}
