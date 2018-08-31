using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Chapter2 //objective 2.6
{
    class LifeCycleManagement
    {
        static WeakReference data;

        public static void Run()
        {
            object result = GetData();
            //GC.Collect(); <== uncomment this line and data.Target will go null
            result = GetData();
        }

        private static object GetData()
        {
            if (data == null)
            {
                data = new WeakReference(LoadLargeList());
            }

            if (data.Target == null)
            {
                data.Target = LoadLargeList();
            }

            return data.Target;
        }

        private static IEnumerable<string> LoadLargeList()
        {
            return null;
        }

        public static void UsingStatementDemo()
        {
            using (StreamWriter sw = File.CreateText("temp.dat")) { }
        }

        public static void CallIDisposeDemo()
        {
            StreamWriter stream = File.CreateText("temp.dat");
            stream.Write("some data");
            stream.Dispose();
            File.Delete("temp.dat");
        }
        public static void ForceGarbageCollection()
        {
            StreamWriter stream = File.CreateText("temp.dat");
            stream.Write("some data");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            File.Delete("temp.dat");
        }
        public static void FileClosingError()
        {
            StreamWriter stream = File.CreateText("temp.dat");
            stream.Write("some data");

            File.Delete("temp.dat"); //would throw IOException. File is already open.
        }
    }

    class UnmanagedWrapper : IDisposable
    {
        private IntPtr unmanagedBuffer;
        public FileStream Stream { get; private set; }
        public UnmanagedWrapper()
        {
            CreateBuffer();
            this.Stream = File.Open("temp.dat", FileMode.Create);
        }

        private void CreateBuffer()
        {
            byte[] data = new byte[1024];
            new Random().NextBytes(data);
            unmanagedBuffer = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data, 0, unmanagedBuffer, data.Length);
        }

        ~UnmanagedWrapper()
        {

        }

        protected virtual void Dispose(bool disposing)
        {
            Marshal.FreeHGlobal(unmanagedBuffer);
            if (disposing)
            {
                if (Stream != null)
                {
                    Stream.Close();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
        }
    }

    public interface IDisposable
    {
        void Dispose();
    }

    public class SomeType
    {
        //finalizer
        ~SomeType()
        {
            //this code gets called when the finalize method executes
        }
    }

}
/*
 * 
 * Thought Experiment: You have created your first Windows 8 app. It's a nice game that enables users to take a video of themselves
 * describing a word. Others have to guess; that way, they can earn points that enable them to create a longer video.
 * 
 * One day you wake up and you suddenly realize that Microsoft chose your app as its app of the week. Your web server that's running
 * all the logic of the game is trembling under the user load because of the sudden popularity. Both memory and CPU pressure are
 * a lot higher than you expected. You have some types that are qualified to be a value type but at the time of creating your
 * app, you just used classes.
 * 
 * 1. How can using value types when possible improve your performance? Or could it be that your performance will deteriorate more?
 *  Value types typically reside on the stack which has a much faster access time (?)
 * 2. Why is implementing IDisposable important to reduce memory pressure? Is it always the best to call Dispose on an element as soon
 *  as you are done with it?
 *      IDisposable will free up memory but it is not always wise to call this method if the finalize method has not yet been called as
 *      you cannot dispose of an object before this.
 * 3. Should you implement a finalizer on all your types that implement IDisposable?
 *  For smaller operations you can implement the Using keyword instead.
 * 
 * 1. You are about to execute a piece of code that is performance-sensitive. You are afraid that a garbage collection will
 * occur during the execution of this code. Which method should you call before executing your code?
 *  2. GC.SuppressFinalize()
 *  
 * 2. An object that is implementing IDisposable is passed to your class as an argument. Should you wrap the element in a using statement?
 *  4. No, the calling method shoud use a using statement.
 * 
 * 3. Your application is using a lot of memory. Which solution should you use?
 *  4. Use a background thread to call GC.Collection() on a scheduled interval.
 * 
 * */
