using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Chapter1
{
    public class UsingThreads
    {
        [ThreadStatic]
        public static int _field;

        public static ThreadLocal<int> _field0 = new ThreadLocal<int>(() =>
        {
            return Thread.CurrentThread.ManagedThreadId;
        });

        public static void ConcurrentDictionaryDemo()
        {
            var dict = new ConcurrentDictionary<string, int>();
            if (dict.TryAdd("k1", 42))
            {
                Console.WriteLine("Added");
            }
            if (dict.TryUpdate("k1", 21, 42))
            {
                Console.WriteLine("42 updated to 21");
            }

            dict["k1"] = 42; //unconditional overwrite

            int r1 = dict.AddOrUpdate("k1", 3, (s, i) => i * 2);
            Console.WriteLine("r1 is {0}", r1);
            int r2 = dict.GetOrAdd("k2", 3);
        }

        public static void ConcurrentQueueDemo()
        {
            ConcurrentQueue<int> queue = new ConcurrentQueue<int>();
            queue.Enqueue(42);

            int result;

            if (queue.TryDequeue(out result))
                Console.WriteLine("Dequeued: {0}", result);


        }

        public static void UsingConcurrentStackDemo()
        {
            ConcurrentStack<int> stack = new ConcurrentStack<int>();

            stack.Push(42);

            int result;

            if (stack.TryPop(out result))
                Console.WriteLine("Popped: {0}", result);

            stack.PushRange(new int[] { 1, 2, 3 });

            int[] values = new int[2];
            stack.TryPopRange(values);

            foreach (int i in values)
                Console.WriteLine(i);
        }

        public static void EnumeratingConcurrentBagDemo()
        {
            ConcurrentBag<int> bag = new ConcurrentBag<int>();

            Task.Run(() =>
            {
                bag.Add(42);
                Thread.Sleep(1000);
                bag.Add(21);
            });
            Task.Run(() =>
            {
                foreach (int i in bag)
                    Console.WriteLine(i);
            }).Wait();
        }

        public static void ConcurrentBagDemo()
        {
            ConcurrentBag<int> bag = new ConcurrentBag<int>();

            bag.Add(42);
            bag.Add(21);

            int result;

            if (bag.TryTake(out result))
                Console.WriteLine(result);

            if (bag.TryPeek(out result))
                Console.WriteLine("There is another item: {0}", result);
        }

        public static void GetConsumableEnumerable()
        {
            BlockingCollection<string> col = new BlockingCollection<string>();
            Task read = Task.Run(() =>
            {
                foreach (string v in col.GetConsumingEnumerable())
                    Console.WriteLine(v);
            });
        }

        public static void BlockingCollectionDemo()
        {
            BlockingCollection<string> col = new BlockingCollection<string>();

            Task read = Task.Run(() =>
            {
                while (true)
                {
                    Console.WriteLine(col.Take());
                }
            });

            Task write = Task.Run(() =>
            {
                while (true)
                {
                    string s = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(s)) break;
                    col.Add(s);
                }
            });

            write.Wait();
        }

        public static void AggregateExceptionDemo()
        {
            var numbers = Enumerable.Range(0, 20);

            try
            {
                var parallelResult = numbers.AsParallel()
                    .Where(i => IsEven(i));

                parallelResult.ForAll(e => Console.WriteLine(e));
            }

            catch (AggregateException e)
            {
                Console.WriteLine("There were {0} exceptions", e.InnerExceptions.Count);
            }
        }

        public static bool IsEven(int i)
        {
            if (i % 10 == 0) throw new ArgumentException("i");

            return i % 2 == 0;
        }

        public static void UsingForAllDemo()
        {
            var numbers = Enumerable.Range(0, 20);

            var parallelResult = numbers.AsParallel()
                .Where(i => i % 2 == 0);

            parallelResult.ForAll(e => Console.WriteLine(e));
        }

        public static void AsSequentialDemo()
        {
            var numbers = Enumerable.Range(0, 20);

            var parallelResult = numbers.AsParallel().AsOrdered()
                .Where(i => i % 2 == 0).AsSequential();

            foreach (int i in parallelResult.Take(5))
                Console.WriteLine(i);
        }

        public static void OrderedParallelQueryDemo()
        {
            var numbers = Enumerable.Range(0, 100);
            var parallelResult = numbers.AsParallel().AsParallel()
                .Where(i => i % 2 == 0)
                .ToArray();

            foreach (int i in parallelResult)
                Console.WriteLine(i);
        }

        public static void UnorderedParallelQueryDemo()
        {
            var numbers = Enumerable.Range(0, 100);
            var parallelResult = numbers.AsParallel()
                .Where(i => i % 2 == 0)
                .ToArray();

            foreach (int i in parallelResult)
                Console.WriteLine(i);
        }

        public static void AsParallelDemo()
        {
            var numbers = Enumerable.Range(0, 100000000);
            var parallelResult = numbers.AsParallel()
                .Where(i => i % 2 == 0)
                .ToArray();
        }

        private async void Button_Click0(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();

            string content = await httpClient
                .GetStringAsync("http://www.microsoft.com")
                .ConfigureAwait(false);

            using (FileStream sourceStream = new FileStream("temp.html",
                FileMode.Create, FileAccess.Write, FileShare.None, 4096, useAsync: true))
            {
                byte[] encodedText = Encoding.Unicode.GetBytes(content);
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length)
                    .ConfigureAwait(false);
            };
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            HttpClient httpClient = new HttpClient();

            string content = await httpClient
                .GetStringAsync("http://www.microsoft.com")
                .ConfigureAwait(false);

            //for WPF only. Not suitable for console app but would cause exception due to ConfigureAwait(false)
            //Output.Content = content;
        }

        public Task SleepAsyncA(int millisecondsTimeout)
        {
            return Task.Run(() => Thread.Sleep(millisecondsTimeout));
        }

        public Task SleepAsyncB(int millisecondsTimeout)
        {
            TaskCompletionSource<bool> tcs = null;
            var t = new Timer(delegate { tcs.TrySetResult(true); }, null, -1, -1);
            tcs = new TaskCompletionSource<bool>(t);
            t.Change(millisecondsTimeout, -1);
            return tcs.Task;
        }

        public static void AsyncAndAwaitDemo()
        {
            string result = DownloadContent().Result;
            Console.WriteLine(result);
        }

        public static async Task<String> DownloadContent()
        {
            using (HttpClient client = new HttpClient())
            {
                string result = await client.GetStringAsync("http://www.microsoft.com");
                return result;
            }
        }

        public static void ParallelBreakDemo()
        {
            int count = 0;
            ParallelLoopResult result = Parallel.
                For(0, 1000, (int i, ParallelLoopState loopState) =>
                {
                    if (i == 50)
                    {
                        Console.WriteLine("50 encountered. Breaking loop.");
                        loopState.Break();
                    }
                    else
                        Console.WriteLine("Parallel Processing: {0}", i);

                    count++;
                    return;
                });

            Console.WriteLine("{0} total loops processed.", count);
        }

        public static void ParallelForAndForeachDemo()
        {
            Parallel.For(0, 10, i =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("For: {0}", i);
            });

            var numbers = Enumerable.Range(0, 10);

            Parallel.ForEach(numbers, i =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("ForEach: {0}", i);
            });
        }

        public static void WaitAnyDemo()
        {
            Task<int>[] tasks = new Task<int>[3];

            tasks[0] = Task.Run(() => { Thread.Sleep(2000); return 1; });
            tasks[1] = Task.Run(() => { Thread.Sleep(1000); return 2; });
            tasks[2] = Task.Run(() => { Thread.Sleep(3000); return 3; });

            while (tasks.Length > 0)
            {
                int i = Task.WaitAny(tasks);
                Task<int> completedTask = tasks[i];

                Console.WriteLine(completedTask.Result);

                var temp = tasks.ToList();
                temp.RemoveAt(i);
                tasks = temp.ToArray();
            }
        }

        public static void WaitAllDemo()
        {
            Task[] tasks = new Task[3];

            tasks[0] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("1");
                return 1;
            });

            tasks[1] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("2");
                return 2;
            });

            tasks[2] = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("3");
                return 3;
            });

            Task.WaitAll(tasks);
        }

        public static void TaskFactoryDemo()
        {
            Task<Int32[]> parent = Task.Run(() =>
            {
                var results = new Int32[3];

                TaskFactory tf = new TaskFactory(TaskCreationOptions.AttachedToParent,
                    TaskContinuationOptions.ExecuteSynchronously);

                tf.StartNew(() => results[0] = 0);
                tf.StartNew(() => results[1] = 1);
                tf.StartNew(() => results[2] = 2);
                return results;
            });

            var finalTask = parent.ContinueWith(
                parentTask =>
                {
                    foreach (int i in parentTask.Result)
                        Console.WriteLine(i);
                });

            finalTask.Wait();
        }

        public static void TaskWithChildTasksDemo()
        {
            Task<Int32[]> parent = Task.Run(() =>
            {
                var results = new Int32[3];
                new Task(() => results[0] = 0,
                    TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[1] = 1,
                    TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[2] = 2,
                    TaskCreationOptions.AttachedToParent).Start();

                return results;
            });

            var finalTask = parent.ContinueWith(
                parentTask =>
                {
                    foreach (int i in parentTask.Result)
                        Console.WriteLine(i);
                });

            finalTask.Wait();
        }

        public static void TaskReturnWithMultipleContinuationsDemo()
        {
            Task<int> t = Task.Run(() =>
            {
                return 42;
            });

            t.ContinueWith((i) =>
            {
                Console.WriteLine("Canceled");
            }, TaskContinuationOptions.OnlyOnCanceled);

            t.ContinueWith((i) =>
            {
                Console.WriteLine("Faulted");
            }, TaskContinuationOptions.OnlyOnFaulted);

            var completedTask = t.ContinueWith((i) =>
            {
                Console.WriteLine("Completed");

            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            completedTask.Wait();
        }

        public static void TaskReturnWithContinuationDemo()
        {
            Task<int> t = Task.Run(() =>
            {
                return 42;
            }).ContinueWith((i) =>
            {
                return i.Result * 2;
            });

            Console.WriteLine(t.Result);
        }

        public static void TaskReturnDemo()
        {
            Task<int> t = Task.Run(() =>
            {
                return 42;
            });

            Console.WriteLine(t.Result);
        }

        public static void TaskDemo()
        {
            Task t = Task.Run(() =>
            {
                for (int x = 0; x < 100; x++)
                {
                    Console.Write('*');
                }
            });

            t.Wait();
        }

        public static void ThreadQueuingPoolDemo()
        {
            ThreadPool.QueueUserWorkItem((s) =>
            {
                Console.WriteLine("Working on a thread from threadpool");
            });

            Console.ReadLine();
        }

        public static void ThreadLocalDemo()
        {
            new Thread(() =>
            {
                for (int x = 0; x < _field0.Value; x++)
                {
                    Console.WriteLine("Thread C: {0}", x);
                }
            }).Start();

            new Thread(() =>
            {
                for (int x = 0; x < _field0.Value; x++)
                {
                    Console.WriteLine("Thread D: {0}", x);
                }
            }).Start();
        }

        public static void ThreadStaticDemo()
        {
            new Thread(() =>
            {
                for (int x = 0; x < 10; x++)
                {
                    _field++;
                    Console.WriteLine("Thread A: {0}", _field);
                }
            }).Start();

            new Thread(() =>
            {
                for (int x = 0; x < 10; x++)
                {
                    _field++;
                    Console.WriteLine("Thread B: {0}", _field);
                }
            }).Start();

            Console.ReadKey();
        }

        public static void StoppingThreadDemo()
        {
            bool stopped = false;

            Thread t = new Thread(new ThreadStart(() =>
            {
                while (!stopped)
                {
                    Console.WriteLine("StoppingThreadDemo Thread Running...");
                    Thread.Sleep(1000);
                }
            }));

            t.Start();
            Console.WriteLine("Press any key to exit StoppingThreadDemo");
            Console.ReadKey();

            stopped = true;
            Console.WriteLine("StoppingThreadDemo has been stopped.");
            t.Join();
        }

        public static void ThreadMethod1(object o)
        {
            for (int i = 0; i < (int)o; i++)
            {
                Console.WriteLine("ThreadProc1: {0}", i);
                Thread.Sleep(0);
            }
        }

        public static void ThreadMethodDemo1()
        {
            Thread t = new Thread(new ParameterizedThreadStart(ThreadMethod1));
            t.Start(5);
            t.Join();
        }

        public static void ThreadMethod0()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("ThreadProc0: {0}", i);
                Thread.Sleep(1000);
            }
        }

        public static void ThreadMethodDemo0()
        {
            Thread t = new Thread(new ThreadStart(ThreadMethod0));
            t.IsBackground = true;
            t.Start();

        }

        public static void ThreadMethod()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("ThreadProc: {0}", i);
                Thread.Sleep(0);
            }
        }

        public static void ThreadMethodDemo()
        {
            Thread t = new Thread(new ThreadStart(ThreadMethod));

            t.Start();

            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("Main thread: doing some work.");
                Thread.Sleep(0);
            }

            t.Join();
        }
    }
}

/*Thought Experiments for this section:
 *In this thought experiment, apply what you've learned about this objective. You can find answers to these
 * questions in the Answers section at the end of this chapter
 * 
 * You need to build a new application, and you look into multithreading capabilities.
 * Your application consists of a client application that communicates with a web server.
 * 
 * 1. Explain how multithreading can help with your client application:
 *      Multithreading, when properly implement, can decouple the client application's dependency
 *      on backend processing. It can free up resources that would otherwise be waiting for I/O operations
 *      to complete.
 *      
 * 2. What is the difference between CPU and I/O bound operations?
 *      CPU bound operations require a dedicated processor to complete. I/O operations do not use processing,
 *      and this is where multi-threading can be the most useful. If a thread cannot move forward until I/O has completed,
 *      it is best to free up the thread for actual work until the I/O has finished.
 *      
 * 3. Does multithreading with the TPL offer the same advantages for your server application?
 *      No. The TPL is best used for UI-based resources that would otherwise be blocked by I/O operations
 *      
 * Objective Review:
 *      1. You have a lot of items that need to be processed. For each item, you need to perform a complex calculation.
 *          Which technique should you use?
 *          a. You create a task for each item and then wait until all tasks are finished.
 *          b. You use Parallel.For to process all items concurrently. <==
 *          c. You use async / await to process all items concurrently.
 *          d. You add all items to a BlockingCollection and process them on a thread created by the Thread class.
 *          
 *      2. You are creating a complex query that doesn't require any particular order and you want to run
 *          it in parallel. Which method should you use?
 *          a. AsParallel <==
 *          b. AsSequential
 *          c. AsOrdered
 *          d. WithDegreeOfParallelism
 * 
 *      3. You are working on an ASP.NET application that retrieves some data from another web server and then
 *          writes the response to the database. Should you use async/await?
 *          a. No, both operations depend on external factors. You need to wait until they are finished.
 *          b. No, in a server application you don't have to use async/await. It's only for responsiveness on the client.
 *          c. Yes, this will free your thread to server other requests while waiting for the I/O to complete. <==
 *          d. Yes, this puts your thread to sleep while waiting for I/O so that it doesn't use any CPU.
 * 
 */
