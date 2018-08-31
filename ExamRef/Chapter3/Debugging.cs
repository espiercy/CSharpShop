#define MySymbol

using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;



namespace Chapter3
{
    class Debugging
    {

        //Apply conditional attribute demo
        [Conditional("DEBUG")]
        private static void Log0(string message)
        {
            Console.WriteLine("message");
        }
        public void CallOnlyInDebugDemo()
        {
#if DEBUG
            Log("Step1");
#endif
        }

        private void Log(string message)
        {
            Console.WriteLine("message");
        }
        public void DisableSpecificWarningsDemo()
        {
#pragma warning disable 0162, 0168
            int i;
#pragma warning restore 0162
            while (false) Console.WriteLine("Unreachable Code");
#pragma warning restore

        }
        public void PragmaDemo()
        {
#pragma warning disable
            while (false) Console.WriteLine("Unreachable Code");
#pragma warning restore
        }
        public void LineDirectiveDemo()
        {
#line 200 "OtherFileName"
            int a; // line 200
#line default
            int b; //line 18
#line hidden
            int c; //supposedly hidden
            int d;
        }
#warning this code is obsolete

#if (!DEBUG)
#error Debug build is not allowed
#endif
        public Assembly LoadAssembly<T>()
        {
#if !WINRT
            Assembly assembly = typeof(T).Assembly;
#else
            Assembly assembly = typeof(T).GetTypeInfo().Assembly;
#endif
            return assembly;
        }
        public static void UseCustomSymbol()
        {
#if MySymbol
            Console.WriteLine("Custom symbol is defined");
#endif
        }
        public static void DebugDirective()
        {
#if (DEBUG)
            Console.WriteLine("Debug mode");
#else
            Console.WriteLine("Not Debug"); 
#endif
        }
    }

    public static class Program0
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            Console.ReadKey();
        }
    }

    //example 3-33
    public static class Program
    {
        public static void Main()
        {
            Timer t = new Timer(TimerCallback, null, 0, 2000);
            Console.ReadLine();
        }

        private static void TimerCallback(Object o)
        {
            Console.WriteLine("In TimerCallback: " + DateTime.Now);
            GC.Collect();
        }
    }

    [DebuggerDisplay("Name = {FirstName} {LastName}")]
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

}
/* You are working in the support department of your organization. A customer phones you to report an error in a web application
 * that you are hosting on your own servers.
* 
* 1. You want to start debugging this application remotely. OK
* 2. Do you need to deploy a debug version to the server? Sure
* 3. What do you need to make this possible? The pdb library / symbol server and some additional error reporting in debug mode using directives
* wouldn't hurt.
* 4. How can a symbol server help you? By figuring out where precisely (in what .dll(s)) code is going wrong.
* 
* Review
You are ready to deploy your code to a production server. Which configuration do you deploy?
2. Release configuration.

    You are debugging an application for a web shop and are inspecting a lot of Order classes. What can you do to make your debugging easier?
    Use the ConditionalAttribute on the order class (?)

    3. You are using custom code generation to insert security checks into your classes. When an exception happens you're having trouble finding the
correct line in your source code. What should you do? 1. Use #error to signal the error from your code so that it's easier to find.
 */
