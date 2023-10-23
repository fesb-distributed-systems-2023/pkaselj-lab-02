using System;
using System.Threading;

namespace Threads.ManagingThreads
{
    public static class Program
    {
        public static void Main()
        {
            int mid = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("Primary Thread ({0}) ", mid);

            var delay = TimeSpan.FromSeconds(10);
            var cancellationTokeSource = new CancellationTokenSource(delay);

            var code = new ConcurrentCode();
            var threads = new[]
            {
                new Thread(new ThreadStart(code.SleepWhileCodeIsNotDone)),
                // new Thread(() => code.SleepWhileCancellationIsNotRequested(cancellationTokeSource.Token)),
                // new Thread(code.DoSleepForLongTime),
            };


            foreach (var thread in threads)
            {
                thread.Start();
            }

            Thread.Sleep(3000);

            foreach (var thread in threads)
            {
                if (thread.IsAlive)
                {
                    Console.WriteLine($"Primary: Thread {thread.ManagedThreadId} is alive.");
                }
            }

            // TODO: Stop the threads (in code or debugger)
            Thread.Sleep(TimeSpan.FromSeconds(10));
            // code.done = true;
            // threads[2].Interrupt();

            foreach (var thread in threads)
            {
                thread.Join();
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Main: All threads have terminated.");

            Console.ResetColor();
        }
    }
}