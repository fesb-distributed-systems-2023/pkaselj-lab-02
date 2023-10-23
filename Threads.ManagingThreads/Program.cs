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

            Thread workerThread;
            var code = new ConcurrentCode();

            workerThread = new Thread (code.SleepWhileCodeIsNotDone);
            workerThread.Start();
            Thread.Sleep(5000); // Wait some time
            code.done = true;
            workerThread.Join();

            // Token that will self-cancel after 3 seconds
            var cancelTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(3)); 
            // Action that will be executed when cancelation is requested
            cancelTokenSource.Token.Register(() => { Console.WriteLine("Cancelation token activated!"); }); 
            workerThread = new Thread(() => { code.SleepWhileCancellationIsNotRequested(cancelTokenSource.Token); });
            workerThread.Start();
            workerThread.Join();


            workerThread = new Thread(code.DoSleepForLongTime); // Create an infinite thread
            workerThread.Start();
            Thread.Sleep(5000); // Wait some time
            workerThread.Interrupt();
            workerThread.Join();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Main: All threads have terminated.");

            Console.ResetColor();
        }
    }
}