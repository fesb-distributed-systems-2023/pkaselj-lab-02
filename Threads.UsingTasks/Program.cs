using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    private static int DoComputeSum(int num)
    {
        var managedThreadId = Thread.CurrentThread.ManagedThreadId;
        var total = 0;
        for (int i = 0; i < num; i++)
        {
            total += i;
            Console.WriteLine($"Thread {managedThreadId}, i: {i} total= {total}");
            Thread.Sleep(100);
        }
        return total;
    }

    private static void DoVeryLongAction_NoThrow(CancellationToken token)
    {
        Console.WriteLine("Started very long action.");

        while (true)
        {
            if (token.IsCancellationRequested)
            {
                break;
            }
            Console.WriteLine("Still performing the action...");
            Thread.Sleep(1000);
        }

        Console.WriteLine("Action interrupted!");
    }

    private static void DoVeryLongAction_ThrowException(CancellationToken token)
    {
        Console.WriteLine("Started very long action.");

        while (true)
        {
            token.ThrowIfCancellationRequested();
            Console.WriteLine("Still performing the action...");
            Thread.Sleep(1000);
        }

        Console.WriteLine("Action interrupted!");
    }

    public static void Main()
    {
        var managedThreadId = Environment.CurrentManagedThreadId;
        
        /* Waiting for a simple task to finish */
        var task = new Task<int>(() => DoComputeSum(100));
        task.Start();
        task.Wait();
        Console.WriteLine($"Task finished with the result: '{task.Result}' ");

        /* Interrupting tasks */
        Task veryLongTask;
        CancellationTokenSource cancelTokenSource;

        // Interrupting by setting a flag
        cancelTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        veryLongTask = new Task(() => { DoVeryLongAction_NoThrow(cancelTokenSource.Token); });
        veryLongTask.Start();
        veryLongTask.Wait();

        // Interrupting by throwing an execption
        cancelTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        veryLongTask = new Task(() => { DoVeryLongAction_ThrowException(cancelTokenSource.Token); });
        veryLongTask.Start();
        try
        {
            veryLongTask.Wait();
            Console.WriteLine("Task exited without problems.");
        }
        catch (AggregateException ex)
        {
            Console.WriteLine($"Caught exception: '{ex.Message}' from: \n{ex.StackTrace}");
        }
    }
}