using System;
using System.Threading;
using System.Threading.Tasks;

namespace Chapter1
{
    public class ManageMultiThreading //Objective 1.2
    {
        private static int _flag = 0;
        private static int _value = 0;
        private static int _compareAndExchangeInt = 1;

        public static void TimeoutDemo()
        {
            Task longRunning = Task.Run(() =>
            {
                Thread.Sleep(10000);
            });

            int index = Task.WaitAny(new[] { longRunning }, 1000);

            if (index == -1)
                Console.WriteLine("Task timed out");
        }
        public static void ContinuationDemo()
        {
            //I don't think this example works at all...
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;
            Task task = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    Console.Write("*");
                    Thread.Sleep(1000);
                }

                throw new OperationCanceledException();
            }, token).ContinueWith((t) =>
            {
                t.Exception.Handle((e) => true);
                Console.WriteLine("You have canceled the task");
            }, TaskContinuationOptions.OnlyOnCanceled);

        }
        public static void OperationCanceledExceptionDemo()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            Task task = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    Console.Write("*");
                    Thread.Sleep(1000);
                }

                token.ThrowIfCancellationRequested();
            }, token);

            try
            {
                Console.WriteLine("Press enter to stop the task");
                Console.ReadLine();

                cancellationTokenSource.Cancel();
                task.Wait();
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.InnerExceptions[0].Message);
            }
            Console.WriteLine("Press enter to end the application.");
            Console.ReadLine();
        }
        public static void CancellationTokenDemo()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = cancellationTokenSource.Token;

            Task task = Task.Run(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    Console.Write("*");
                    Thread.Sleep(1000);
                }
            }, token);

            Console.WriteLine("Press Enter to stop the task");
            Console.ReadLine();

            cancellationTokenSource.Cancel();

            Console.WriteLine("Press enter to end the applicaiton.");
            Console.ReadLine();

        }
        public static void CompareAndExchangeDemo()
        {
            Task t1 = Task.Run(() =>
           {
               if (_compareAndExchangeInt == 1)
               {
                   //removing line below will change output
                   Thread.Sleep(1000);
                   _compareAndExchangeInt = 2;
               }
           });

            Task t2 = Task.Run(() =>
            {
                _compareAndExchangeInt = 3;
            });

            Task.WaitAll(t1, t2);
            Console.WriteLine(_compareAndExchangeInt);
        }
        public static void InterlockedDemo()
        {
            int n = 0;

            var up = Task.Run(() =>
            {
                for (int i = 0; i < 1000000; i++)
                    Interlocked.Increment(ref n);
            });

            for (int i = 0; i < 1000000; i++)
                Interlocked.Decrement(ref n);

            up.Wait();
            Console.WriteLine(n);
        }
        public static void Thread2()
        {
            if (_flag == 1)
                Console.WriteLine(_value);
        }
        public static void Thread1()
        {
            _value = 5;
            _flag = 1;
        }
        public static void GeneratedFromLockDemo()
        {
            //this is just an example of the code generated from the lock statement. Don't write it out in practice
            object gate = new object();
            bool _lockTaken = false;
            try
            {
                Monitor.Enter(gate, ref _lockTaken);
            }
            finally
            {
                if (_lockTaken)
                    Monitor.Exit(gate);
            }
        }
        public static void DeadLockDemo() //does not cause deadlock
        {
            object lockA = new object();
            object lockB = new object();

            var up = Task.Run(() =>
            {
                lock (lockA)
                {
                    Thread.Sleep(1000);
                    lock (lockB)
                    {
                        Console.WriteLine("Locked A and B");
                    }
                }
            });

            lock (lockB)
            {
                lock (lockA)
                {
                    Console.WriteLine("Locked B and A");
                }
            }
            up.Wait();
        }
        public static void LockKeyWordDemo()
        {
            int n = 0;

            object _lock = new object();

            var up = Task.Run(() =>
            {
                for (int i = 0; i < 1000000; i++)
                    lock (_lock)
                        n++;
            });

            for (int i = 0; i < 1000000; i++)
                lock (_lock)
                    n--;

            up.Wait();

            Console.WriteLine(n);
        }
        public static void SynchingResourcesDemo()
        {
            int n = 0;

            var up = Task.Run(() =>
            {
                for (int i = 0; i < 1000000; i++)
                    n++;
            });

            for (int i = 0; i < 1000000; i++)
                n--;

            up.Wait();
            Console.WriteLine(n);
        }
    }

    /*Thought Experiment: You are experiencing deadlocks in your code. It's true that you have a lot of locking statements and you are trying to improve
     * your code to avoid the deadlocks.
     * 
     * 1. How can you orchestrate your locking code to avoid deadlocks? Make certain your locks are requested in the same order so that the first thread finishes
     *      all work before releasing the lock, allowing the second thread to continue, etc.
     * 2. How can the interlocked class help you? This ensures that a resource being used by more than one thread is only being used in one place at a time.
     * 
     * Review:
     * 
     * 1. You want to synchronize access by using a lock statement. On which member do you lock?
     *      a. this
     *      b. string_lock = "mylock"
     *      c. int_lock = 42;
     *      d. object _lock = new object(); <==
     *      
     * 2. You need to implement cancellation for a long running task. Which object do you pass to the task?
     *      a. CancellationTokenSource
     *      b. CancellationToken <==
     *      c. Boolean isCancelled variable
     *      d. Volatile
     *      
     * 3. You are implementing a state machine in a multithreaded class. You need to check what the current state is and change it to the
     *  new one on each step. Which method do you use?
     *  
     *      a. Volatile.Write(ref currentState)
     *      b. Interlocked.CompareExchange(ref currentState, ref newState, expectedState) <==
     *      c. Interlocked.Exchange(ref currentState, newState)
     *      d. Interlocked.Decrement(ref newState)
     */
}
