using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCH3CodeChallenges
{
    class Jet : Transformer
    {
        public Jet()
        {
            Wheels = 8;
            Speed = 900;
            Run();
        }
    }
}
