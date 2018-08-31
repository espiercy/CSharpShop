using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Chapter1
{
    public class EventsAndCallbacks //Objective 1.4
    {
        public delegate void ContravarianceDel(StreamWriter tw);
        public delegate TextWriter CovarianceDel();
        public delegate void Del();
        public delegate int Calculate(int x, int y);

        public static void EventExceptionHandlingDemo()
        {
            PubWithExceptionHandling p = new PubWithExceptionHandling();

            p.OnChange += (sender, e) => Console.WriteLine("Subscriber 1 called");
            p.OnChange += (sender, e) => { throw new Exception(); }; //again, this doesn't work...
            p.OnChange += (sender, e) => Console.WriteLine("Subscriber 3 called");

            try
            {
                p.Raise();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.InnerExceptions.Count);
            }
        }
        public static void EventHandlerExceptionDemo()
        {
            PubWithException p = new PubWithException();

            p.OnChange += (sender, e) => Console.WriteLine("Subscriber 1 called");
            p.OnChange += (sender, e) => { throw new Exception(); };
            p.OnChange += (sender, e) => Console.WriteLine("Subscriber 3 called");
            p.Raise();
        }
        public static void EventArgsDemo()
        {
            PubWithEventArgs p = new PubWithEventArgs();
            p.OnChange += (sender, e) => Console.WriteLine("Event raised: {0} Sender: {1}", e.Value, sender.GetType());

            p.Raise();
        }
        public static void PublishSubscribeDemo()
        {
            Pub p = new Pub();
            p.OnChange += () => Console.WriteLine("Event raised to method 1");
            p.OnChange += () => Console.WriteLine("Event raised to method 2");

            p.Raise();

            //gen Pub instance, subscribes to event, raises event w/p.Raise()
        }
        public static void ActionDemo()
        {
            //actions return void
            Action<int, int> calc = (x, y) =>
             {
                 Console.WriteLine(x + y);
             };
            calc(3, 4);
        }
        public static void LambdaWithMultipleStatementsDemo()
        {
            Calculate calc = (x, y) =>
            {
                Console.WriteLine("Adding numbers");
                return x + y;
            };

            int result = calc(3, 4);
        }
        public static void LambdaDemo()
        {
            Calculate calc = (x, y) => x + y; //equivalent of delegate we saw earlier
            Console.WriteLine(calc(3, 4));

            calc = (x, y) => x * y; //equivalent of delegate we saw earlier
            Console.WriteLine(calc(3, 4));
        }
        public static void ContravarianceDemo()
        {
            ContravarianceDel del = DoSomething;
            //possible because ContravarianceDel takes a StreamWriter, and TextWriter inherits from StreamWriter
        }
        public static void CovarianceDemo()
        {
            CovarianceDel del;
            del = MethodStream;
            del = MethodString;

            //possible because MethodStream and MethodString both inherit from Textwriter.
        }
        public static void MulticastingDemo()
        {
            Del d = MethodOne;
            d += MethodTwo;

            d();
        }
        public static void DelegateDemo()
        {
            Calculate calc = Add;
            Console.WriteLine(calc(3, 4));

            calc = Multiply;
            Console.WriteLine(calc(3, 4));
        }

        public static void DoSomething(TextWriter tw) { }
        public static StreamWriter MethodStream() { return null; }
        public static StringWriter MethodString() { return null; }
        public static void MethodOne()
        {
            Console.WriteLine("MethodOne");
        }
        public static void MethodTwo()
        {
            Console.WriteLine("MethodTwo");
        }
        public static int Add(int x, int y) { return x + y; }
        public static int Multiply(int x, int y) { return x * y; }
    }

    public class PubWithExceptionHandling
    {
        public event EventHandler OnChange = delegate { };

        public void Raise()
        {
            var exceptions = new List<Exception>();

            foreach (Delegate handler in OnChange.GetInvocationList())
            {
                try
                {
                    handler.DynamicInvoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }
    }
    public class PubWithException
    {
        public event EventHandler OnChange = delegate { };

        public void Raise()
        {
            OnChange(this, EventArgs.Empty);
        }
    }
    public class PubWithCustomEventAccessor
    {
        private event EventHandler<MyArgs> onChange = delegate { };

        public event EventHandler<MyArgs> OnChange
        {
            add
            {
                lock (onChange)
                {
                    onChange += value;
                }
            }
            remove
            {
                lock (onChange)
                {
                    onChange -= value;
                }
            }
        }

        public void Raise()
        {
            onChange(this, new MyArgs(42));
        }
    }
    public class MyArgs : EventArgs
    {
        public MyArgs(int value)
        {
            Value = value;
        }

        public int Value { get; set; }
    }
    public class PubWithEventArgs
    {
        public event EventHandler<MyArgs> OnChange = delegate { };

        public void Raise()
        {
            OnChange(this, new MyArgs(42));
        }
    }
    public class PubWithEventKeyWord
    {
        public event Action OnChange = delegate { };

        public void Raise()
        {
            OnChange();
        }
    }
    public class Pub
    {
        public Action OnChange { get; set; }

        public void Raise()
        {
            if (OnChange != null)
            {
                OnChange();
            }
        }
    }

    /*
     Thought experiment: You are working on a desktop application that consists of mutliple forms. Those forms show different
     views of the same data and they should update in real time. Your application is extensible, and third parties can add
     plug-ins that contain their own views of the data.

    1. Should you use delegates or events in this system? Events
    2. How can this help you? This will protect the third-party plugins from subscribing/unsubscribing events and protect your code.

    Review:
    1. You have a private method in your class and you want to make the invocation of the method possible by certain callers.
    What do you do?
        a. Make the method public.
        b. Use an event so outside users can be notified when the method is executed.
        c. Use a method that returns a delegate to authorized callers. <=
        d. Declare the private method as a lambda.

    2. You have declared an event on your class, and you want outside users of your class to raise this event. What do you do?
        1. Make the event public.
        2. Add a public method to your class that raises the event <=
        3. Use a public delegate instead of an event.
        4. Use a custom event accessor to give access to outside users <== more specific

    3. You are using a multicast delegate with multiple subscribers. You want to make sure that all subscribers are notified,
    even if an exception is thrown. What do you do?
        1. Manually raise the events by using GetInvocationList <== more specific
        2. Wrap the raising of the event in a try/catch
        3. Nothing. This is the default behavior.
        4. Let subscribers return true or false instead of throwing an exception.
     */
}
