using System;
using System.Collections.Generic;

namespace Chapter1
{
    class ProgramFlow
    {
        public static void GotoDemo()
        {
            int x = 3;
            if (x == 3) goto customLabel;
            x++;
            customLabel:
            Console.WriteLine(x);
        }
        public static void ForeachCompilerCodeDemo()
        {
            var people = new List<Person>
            {
                new Person() {FirstName = "John", LastName = "Doe" },
                new Person() {FirstName = "Jane", LastName = "Doe" },
            };

            List<Person>.Enumerator e = people.GetEnumerator();

            try
            {
                Person v;
                while (e.MoveNext())
                {
                    v = e.Current;
                }
            }
            finally
            {
                System.IDisposable d = e as System.IDisposable;
                if (d != null) d.Dispose();
            }
        }
        public static void ChangingInForeachDemo()
        {
            var people = new List<Person>
            {
                new Person() {FirstName = "John", LastName = "Doe" },
                new Person() {FirstName = "Jane", LastName = "Doe" },
            };

            foreach (Person p in people)
            {
                p.LastName = "Changed"; //allowed
                //p = new Person(); <- compiler error
            }
        }
        public static void ForeachDemo()
        {
            int[] values = { 1, 2, 3, 4, 5, 6 };
            foreach (int i in values)
            {
                Console.WriteLine(i);
            }
        }
        public static void DoWhileDemo()
        {
            do
            {
                Console.WriteLine("Executed once!");
            }
            while (false);
        }
        public static void WhileDemo()
        {
            int[] values = { 1, 2, 3, 4, 5, 6 };
            {
                int index = 0;
                while (index < values.Length)
                {
                    Console.WriteLine(values[index]);
                    index++;
                }
            }
        }
        public static void ForWithContinueStatement()
        {
            int[] values = { 1, 2, 3, 4, 5, 6 };

            for (int index = 0; index < values.Length; index++)
            {
                if (values[index] == 4) continue;

                Console.WriteLine(values[index]);
            }
        }
        public static void ForWithBreakStatement()
        {
            int[] values = { 1, 2, 3, 4, 5, 6 };

            for (int index = 0; index < values.Length; index++)
            {
                if (values[index] == 4) break;

                Console.WriteLine(values[index]);
            }
        }
        public static void ForWithCustIncrement()
        {
            int[] values = { 1, 2, 3, 4, 5, 6 };
            for (int index = 0; index < values.Length; index += 2)
            {
                Console.WriteLine(values[index]);
            }
        }
        public static void ForWithMultVarsDemo()
        {
            int[] values = { 1, 2, 3, 4, 5, 6 };
            for (int x = 0, y = values.Length - 1; ((x < values.Length) && (y >= 0)); x++, y--)
            {
                Console.WriteLine(values[x]);
                Console.WriteLine(values[y]);
            }
        }
        public static void ForDemo()
        {
            int[] values = { 1, 2, 3, 4, 5, 6 };
            for (int index = 0; index < values.Length; index++)
            {
                Console.Write(values[index]);
            }
        }
        public static void GotoInSwitchDemo()
        {
            int i = 1;
            switch (i)
            {
                case 1:
                    {
                        Console.WriteLine("Case 1");
                        goto case 2;
                    }
                case 2:
                    {
                        Console.WriteLine("Case 2");
                        break;
                    }
            }
        }
        public static void SwitchDemo(char input)
        {
            switch (input)
            {
                case 'a':
                case 'e':
                case 'i':
                case 'o':
                case 'u':
                    {
                        Console.WriteLine("Input is a vowel");
                    }
                    break;
                default:
                    {
                        Console.WriteLine("Input is a consonant");
                    }
                    break;
            }
        }
        public static void CheckCharDemo(char input)
        {
            if (input == 'a'
                || input == 'e'
                || input == 'i'
                || input == 'o'
                || input == 'u')
            {
                Console.WriteLine("Input is a vowel.");
            }
            else
            {
                Console.WriteLine("Input is a consonant");
            }
        }
        public static int ConditionalDemo(bool p)
        {
            if (p)
                return 1;
            else
                return 0;

            //return p ? 1 : 0;
        }
        public static void NestedNullCoalescingDemo()
        {
            int? x = null;
            int? z = null;
            int y = x ?? z ?? -1;
        }
        public static void NullCoalescingDemo()
        {
            int? x = null;
            int y = x ?? -1;
        }
        public static void MoreReadableIfDemo()
        {
            bool x = true;
            bool y = true;
            if (x)
            {
                if (y)
                {
                    Console.WriteLine("x and y");
                }
                else
                {
                    Console.WriteLine("x and anot y");
                }
            }
        }
        public static void MultElseDemo()
        {
            bool b = false;
            bool c = true;

            if (b)
            {
                Console.WriteLine("b is true");
            }
            else if (c)
            {
                Console.WriteLine("c is true");
            }
            else
            {
                Console.WriteLine("b and c are false.");
            }
        }
        public static void ElseDemo()
        {
            bool b = false;
            if (b)
            {
                Console.WriteLine("True");
            }
            else
            {
                Console.WriteLine("False");
            }
        }
        public static void ScopingDemo()
        {
            bool b = true;

            if (b)
            {
                int r = 42;
                r++;
                b = false;
            }//r++; not accessible outside, b is now false
        }
        public static void IfCodeBlockDemo()
        {
            bool b = true;
            if (b)
            {
                Console.WriteLine("Both lines executed.");
                Console.WriteLine("Both lines executed.");
            }
        }
        public static void IfDemo()
        {
            bool b = true;
            if (b)
                Console.WriteLine("True");
        }
        public static void XorDemo()
        {
            bool a = true;
            bool b = false;

            Console.WriteLine(a ^ a);
            Console.WriteLine(a ^ b);
            Console.WriteLine(b ^ b);
        }
        public static void AndShortCircuitDemo(string input)
        {
            bool result = (input != null) && (input.StartsWith("v")); //...do something else...
        }
        public static void AndDemo()
        {
            int value = 42;
            bool result = (0 < value) && (value < 100);
        }
        public static void OrShortCircuitDemo()
        {
            bool x = true;
            bool result = x || GetY(); //GetY never called. This is what it means to short circuit
        }
        private static bool GetY()
        {
            Console.WriteLine("This method doesn't get called.");
            return true;
        }
        public static void BooleanDemo()
        {
            bool x = true;
            bool y = false;

            bool result = x || y;

            Console.WriteLine(result);
        }
        public static void EqualityDemo()
        {
            int x = 42;
            int y = 1;
            int z = 42;

            Console.WriteLine(x == y);
            Console.WriteLine(x == z);
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    /*
     Thought Experiment: You are updating an old C#2 console application to a WPF C#5 application. The application is used
     by hotels to keep track of reservations and guests coming and leaving. You are going through the old code base to
     determine whether there is code that can be easily reused. You notice:
        The code uses the goto statement to manage flow.
        There are a lot of long if statements that map user input
        The code uses the for loop extensively.

    1. What is the disadvantage of using goto? How can you avoid using the goto statement?
        Goto quickly gets difficult to follow. It is hard to read and easy to direct control to the wrong place
        Goto can be avoid by developing alternative control statements

    2. What statement can you use to improve the long if statements?
        Switch statements

    3. What are the differences between the for and foreach statement? When should you use which?
        For loop takes an index counter and runs through an operation a specified number of times.
        Foreach is guaranteed to move through each object in a list.
        Use for loops for small, predetermined operations. Use foreach for operations where size may not be known and the
        operations do not modify the order of the list.

    Review:
    1. You need to iterate over a collection in which you know the number of items. You need to remove certain items from the
    collection. Which statement do you use?
        a. switch
        b. foreach
        c. for <=
        d. goto

    2. You have a lot of checks in your application for null values. If a value is not null you want to call a method on it.
    You want to simplify your code. Which technique do you use?
        a. for
        b. Conditional operator
        c. Null-coalescing operator
        d. The short-circuiting behavior of the and operator <=

    3. You are processing some data from over the network. You use HasNext and Read method to retrieve the data. You
    need to run some code on each item. What do you use?
        a. for
        b. foreach <=
        3. while
        4. do-while
     */
}
