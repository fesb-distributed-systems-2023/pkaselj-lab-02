using System;
using System.Threading;

namespace Threads.ManagingThreads
{
    public class ConcurrentCode
    {
        public volatile bool done = false;

        public void SleepWhileCodeIsNotDone()
        {
            var managedThreadId = Environment.CurrentManagedThreadId;
            while (!done)
            {
                Console.WriteLine($"SleepWhileCodeIsNotDone ({managedThreadId}): Sleeping for 2s");
                Thread.Sleep(2000);
            }

            Console.WriteLine($"SleepWhileCodeIsNotDone ({managedThreadId}): Finished.");
        }

        public void SleepWhileCancellationIsNotRequested(CancellationToken cancellationToken)
        {
            int managedThreadId = Environment.CurrentManagedThreadId;

            while (!cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine($"SleepWhileCancellationIsNotRequested ({managedThreadId}): Sleeping for 1s");
                Thread.Sleep(10000);
            }

            Console.WriteLine($"SleepWhileCancellationIsNotRequested ({managedThreadId}): Finished.");
        }


        public void DoSleepForLongTime()
        {
            int managedThreadId = Thread.CurrentThread.ManagedThreadId;

            var iterations = 10000000;

            Console.WriteLine($"DoSleepForLongTime ({managedThreadId}): Doing work for {iterations} iterations.");

            Thread.SpinWait(iterations);

            try
            {
                Console.WriteLine($"DoSleepForLongTime ({managedThreadId}): Going to sleep for very long time!");
                Thread.Sleep(Timeout.Infinite);
            }
            catch (ThreadInterruptedException e)
            {
                Console.WriteLine($"DoSleepForLongTime ({managedThreadId}): Sleep interupted!");
                Console.WriteLine($"DoSleepForLongTime ({managedThreadId}): Exception: {e.Message}");
                Console.WriteLine($"DoSleepForLongTime ({managedThreadId}): Finished.");
            }
        }
    }
}