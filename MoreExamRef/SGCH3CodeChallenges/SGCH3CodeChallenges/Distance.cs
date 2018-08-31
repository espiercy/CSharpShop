using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCH3CodeChallenges
{
    //demonstrates operator overloading, and < , >
    class Distance
    {
        public int Meter { get; set; }

        public static Distance operator ++ (Distance dis)
        {
            dis.Meter += 1;
            return dis;
        }

        public static bool operator < (Distance d1, Distance d2)
        {
            return d1.Meter < d2.Meter;
        }

        public static bool operator > (Distance d1, Distance d2)
        {
            return d1.Meter > d2.Meter;
        }
    }
}
