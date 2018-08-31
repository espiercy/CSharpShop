using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCH3CodeChallenges
{
    class Boat : Transformer
    {
        public Boat()
        {
            Wheels = 0;
            Speed = 200;
            Run();
        }
    }
}
