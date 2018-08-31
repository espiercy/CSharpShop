using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCH3CodeChallenges
{
    class Program
    {
        enum Landscape { Air, Road, Water}
        static void Main(string[] args)
        {
            //distance class has overloaded operator ++
            //DistanceIncrementDemo();
            //StudentBinaryDemo();
            //DistanceBooleanDemo();
            TransformerDemo();
        }

        static void DistanceIncrementDemo()
        {
            Distance d = new Distance();
            d.Meter = 5;
            d++;
            Console.WriteLine("Distance Object has been incremented using overloaded operator: {0}", d.Meter);
        }

        static void StudentBinaryDemo()
        {
            Student s1 = new Student { Marks = 10 };
            Student s2 = new Student { Marks = 20 };

            Student s3 = s1 + s2;
            Console.WriteLine("New student created and binary operator applied: {0}", s3.Marks);
        }

        static void DistanceBooleanDemo()
        {
            Distance d1 = new Distance { Meter = 10 };
            Distance d2 = new Distance { Meter = 20 };
            if (d1 < d2)
            {
                Console.WriteLine("d1 is less than d2");
            }
            else if(d1 > d2)
            {
                Console.WriteLine("d1 is less than d2");
            }
        }

        static void TransformerDemo()
        {
            Transformer t = null;
            foreach(Landscape l in Enum.GetValues(typeof(Landscape)))
            {
                Transform(t, l);
            }
        }

        static Transformer Transform(Transformer t, Landscape l)
        {
            switch (l)
            {
                case Landscape.Air:
                    t = new Jet();
                    break;
                case Landscape.Road:
                    t = new Car();
                    break;
                case Landscape.Water:
                    t = new Boat();
                    break;
                default:
                    t = null;
                    break;
            }
            return t;
        }
    }
}
