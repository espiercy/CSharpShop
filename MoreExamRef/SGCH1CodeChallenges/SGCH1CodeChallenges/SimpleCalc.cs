using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGCH1CodeChallenges
{
    class SimpleCalc
    {
        private int Choice { get; set; }
        private int Var1 { get; set; }
        private int Var2 { get; set; }

        private bool cont;
        public SimpleCalc()
        {
            cont = true;
            while (cont)
            {
                Choice = GetAnInt("Press any following key to perform an arithmetic operation:\n1 - Addition\n2 - Subtraction\n3 - Multiplication\n4 - Division");

                //check to see if choice is between 1 and 4
                CheckChoice();

                if (cont)
                {
                    Var1 = GetAnInt("Enter Value 1:");
                }

                if (cont)
                {
                    Var2 = GetAnInt("Enter Value 2:");
                }

                if (cont)
                {
                    Console.WriteLine(PerformOperation());
                }

                Console.WriteLine("Do you wish to continue? y/n");
                string contString = Console.ReadLine();
                if (char.Parse(contString.ToLower()) == 'y')
                    cont = true;
                else if (char.Parse(contString) == 'n')
                {
                    Console.WriteLine("Terminating Application");
                    cont = false;
                }
                else
                {
                    Console.WriteLine("Unrecognized command. Application terminating.");
                    cont = false;
                }
            }
        }

        private int GetAnInt(String msg)
        {
            int retVal;
            Console.WriteLine(msg);
            string choice = Console.ReadLine();
            try
            {
                retVal = int.Parse(choice);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                retVal = 0;
                cont = false;
            }
            return retVal;
        }

        private void CheckChoice()
        {
            if (Choice >= 1 && Choice <= 4)
                Console.WriteLine();
            else
            {
                cont = false;
                Console.WriteLine("Invalid numeric choice. Operation cancelled");
            }
            
        }

        private string PerformOperation()
        {
            switch (Choice)
            {
                case 1:
                    {
                        return Var1.ToString() + " + " + Var2.ToString() + " = " + (Var1 + Var2).ToString();
                    }
                case 2:
                    {
                        return Var1.ToString() + " - " + Var2.ToString() + " = " + (Var1 - Var2).ToString();
                    }
                case 3:
                    {
                        return Var1.ToString() + " * " + Var2.ToString() + " = " + (Var1 * Var2).ToString();
                    }
                case 4:
                    {
                        return Var1.ToString() + " / " + Var2.ToString() + " = " + (Var1 / Var2).ToString();
                    }
                default:
                    {
                        return "Error. Default case has been reached";
                    }
            }
        }
    }
}
