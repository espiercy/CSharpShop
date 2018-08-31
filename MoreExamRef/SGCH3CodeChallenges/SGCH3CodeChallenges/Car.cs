using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCH3CodeChallenges
{
    class Car : Transformer
    {
        public Car()
        {
            Wheels = 4;
            Speed = 350;
            Run();
        }
    }
}
