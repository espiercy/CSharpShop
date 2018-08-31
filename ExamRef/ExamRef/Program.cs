using System;
//using static Chapter1.UsingThreads;
using static Chapter4.SerializeDeserialize;

namespace ExamRef
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlSerializerDemo();
            WriteStopMessage();
        }

        private static void WriteStopMessage()
        {
            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }
    }
}
