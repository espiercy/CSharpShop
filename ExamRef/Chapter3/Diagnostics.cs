using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Chapter3
{
    class Diagnostics
    {
        public static void ReadDataFromEventLogDemo()
        {
            EventLog log = new EventLog("MyNewLog");

            Console.WriteLine("Total entries: " + log.Entries.Count);
            EventLogEntry last = log.Entries[log.Entries.Count - 1];
            Console.WriteLine("Index:   " + last.Index);
            Console.WriteLine("Source:   " + last.Source);
            Console.WriteLine("Type:   " + last.EntryType);
            Console.WriteLine("Time:   " + last.TimeWritten);
            Console.WriteLine("Message:   " + last.Message);
        }
        public static void ConfigureTraceListenerDemo()
        {
            Stream outputFile = File.Create("tracefile.txt");
            TextWriterTraceListener textListener = new TextWriterTraceListener(outputFile);

            TraceSource traceSource = new TraceSource("myTraceSource", SourceLevels.All);

            traceSource.Listeners.Clear();
            traceSource.Listeners.Add(textListener);

            traceSource.TraceInformation("Trace output");

            traceSource.Flush();
            traceSource.Close();


        }
        public static void TraceSource()
        {
            TraceSource traceSource = new TraceSource("myTraceSource", SourceLevels.All);

            traceSource.TraceInformation("Tracing application..");
            traceSource.TraceEvent(TraceEventType.Critical, 0, "Critical Trace");
            traceSource.TraceData(TraceEventType.Information, 1, new object[] { "a", "b", "c" });
            traceSource.Flush();
            traceSource.Close();
        }
        public static void DebugClassDemo()
        {
            Debug.WriteLine("Starting application");
            Debug.Indent();
            int i = 1 + 2;
            Debug.Assert(i == 3);
            Debug.WriteLineIf(i > 0, "i is greater than 0");
        }
    }

    class MySample
    {
        public static void Main()
        {
            if (!EventLog.SourceExists("MySource"))
            {
                EventLog.CreateEventSource("MySource", "MyNewLog");
                Console.WriteLine("CreatedEventSource");
                Console.WriteLine("Please restart application");
                Console.ReadKey();
                return;
            }

            EventLog myLog = new EventLog();
            myLog.Source = "MySource";
            myLog.WriteEntry("Log event!");
        }
    }

    class EventLogSample
    {
        public static void Main()
        {
            EventLog applicationLog = new EventLog("Application", ".", "testEventLogEvent");
            applicationLog.EntryWritten += (sender, e) =>
            {
                Console.WriteLine(e.Entry.Message);
            };
            applicationLog.EnableRaisingEvents = true;
            applicationLog.WriteEntry("Test message", EventLogEntryType.Information);

            Console.ReadKey();
        }
    }

    class StopWatchDemo
    {
        const int numberOfIterations = 100000;
        static void Main(string args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Algorithm1();
            sw.Stop();

            Console.WriteLine(sw.Elapsed);

            sw.Reset();
            sw.Start();

            Algorithm2();
            sw.Stop();

            Console.WriteLine(sw.Elapsed);
            Console.WriteLine("Ready...");
            Console.ReadLine();
        }

        private static void Algorithm2()
        {
            string result = "";
            for (int x = 0; x < numberOfIterations; x++)
            {
                result += 'a';
            }
        }

        private static void Algorithm1()
        {
            StringBuilder sb = new StringBuilder();
            for (int x = 0; x < numberOfIterations; x++)
            {
                sb.Append('a');
            }

            string result = sb.ToString();
        }
    }

    class PerformanceCounterDemo
    {
        static void Main(String[] args)
        {
            Console.WriteLine("Press escape key to stop");
            using (PerformanceCounter pc = new PerformanceCounter("Memory", "Available Bytes"))
            {
                string text = "Available memory: ";
                Console.Write(text);
                do
                {
                    while (!Console.KeyAvailable)
                    {
                        Console.Write(pc.RawValue);
                        Console.SetCursorPosition(text.Length, Console.CursorTop);
                    }
                } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            }
        }
    }

    class ReadDataFromPerformanceCounterDemo
    {
        static void Main(String[] args)
        {
            if (CreatePerformanceCounters())
            {
                Console.WriteLine("Created performance counters");
                Console.WriteLine("Please restart application");
                Console.ReadKey();
                return;
            }
            var totalOperationsCounter = new PerformanceCounter(
                "MyCategory",
                "# operations executed",
                "",
                false);
            var operationsPerSecondCounter = new PerformanceCounter(
                "MyCategory",
                "# operations / sec",
                "",
                false);

            totalOperationsCounter.Increment();
            operationsPerSecondCounter.Increment();
        }

        private static bool CreatePerformanceCounters()
        {
            if (!PerformanceCounterCategory.Exists("MyCategory"))
            {
                CounterCreationDataCollection counters = new CounterCreationDataCollection
                {
                    new CounterCreationData(
                       "# operations executed",
                       "Total number of operations executed",
                       PerformanceCounterType.NumberOfItems32),
                };
                PerformanceCounterCategory.Create("MyCategory", "Sample category for Codeproject", counters);

                return true;
            }
            return false;
        }
    }
}

/*
 * Example 3-48 Using a configuration file for tracing
 * 
 * <?xml version="1.0" encoding="utf-8" ?>
 * <configuration>
 *  <system.diagnostics>
 *      <sources>
 *          <source name="myTraceSource" switchName="defaultSwitch">
 *              <listeners>
 *                  <add initializeData="output.txt" type="System.Diagnositcs.TextWriterTraceListener"
 *                      name="myLocalListener">
 *                      <filter type="System.Diagnostics.EventTypeFilter" initializeData="Warning"/>
 *                  <add name="consoleListener" />
 *                  <remove name="Default" />
 *              </listeners>
 *          </source>
 *      </sources>
 *      <sharedListeners>
 *          <add initializeData="output.xml" type="System.Diagnostics.XmlWriterTraceListener" name="xmlListener"
 *              traceOutputOptions="None" />
 *          <add type="System.Diagnostics.ConsoleTraceListener" name="consoleListener" traceOutputOptions="None" />   
 *      </sharedListeners>
 *      <switches>
 *          <add name="defaultSwitch" value="All" />
 *      </switches>
 *  </system.diagnostics>
 * </configuration>
 * 
 * 
 * 
 * */
/*Thought exp:
 * 
 * You are building an online web shop that will be hosted in a distributed environment. Your web shop needs to scale well, so performance
 * is an important concept.
 * 1. Which events would you write to a trace source in a web shop? Failed transactions, lost connections, timeouts
 * 2. How can you use performance counters to keep an eye on your performance: setting them up to warn admins if response times begin
 * to lag significantly or some other critical errors occur.
 * 
 * Review:
 * 1. You are using the TraceSource class to trace a data for your application. You want to trace data when an order cannot be submitted
 * to the database and you are going to perform a retry. Which TraceEventType should you use?
 *  1. Information?
 * 2. Users are reporting errors in your application and you want to configure your application to output more trace data.
 * Which configuration setting should you change? 2. Listener
 * 3. You are working on a global application with lots of users. The operation staff requests information on how many
 * user logons per second are occuring. What should you do? 2. Implement a performance counter using the RateOfCountsPerSecond64 type.
 */
