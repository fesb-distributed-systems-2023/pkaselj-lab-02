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
                new Thread(() => code.SleepWhileCancellationIsNotRequested(cancellationTokeSource.Token)),
                new Thread(code.DoSleepForLongTime),
            };

            for (int i = 0; i < threads.Length; i++)
                threads[i].Start();

            Thread.Sleep(3000);

            bool alive = threads[2].IsAlive;
            if (alive)
            {
                Console.WriteLine($"Primary: Thread {{0}} is alive.", threads[2].ManagedThreadId);
            }

            code.done = true;
            threads[2].Interrupt();

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Main: All threads have terminated.");

            Console.ResetColor();
        }
    }
}