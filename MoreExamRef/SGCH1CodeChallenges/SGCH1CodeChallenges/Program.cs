using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCH1CodeChallenges
{
    class Program
    {
        static void Main(string[] args)
        {
            //SimpleCalc c = new SimpleCalc();
            SimpleRepCard r = new SimpleRepCard();
            r.PrintStudentReport();
        }

        public static int ValidEntryEnforcer(string msg)
        { 
            bool isValid = true;
            int validInt = -1;

            do
            {
                Console.Write(msg);
                try
                {
                    validInt = int.Parse(Console.ReadLine());
                    isValid = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error. Entry cannot be converted to int. Try again.");
                    isValid = false;
                }

                if (!(isValid && validInt >= 0 && validInt <= 100))
                {
                    isValid = false;
                    Console.WriteLine("Error. Entry is out of bounds try again.");
                }
                    
            } while (!isValid);
            return validInt;
        }
    }
}
