using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    private static int DoCompute(int num, CancellationToken token)
    {
        var managedThreadId = Thread.CurrentThread.ManagedThreadId;
        var total = 0;
        for (int i = 0; i < num; i++)
        {
            total += i;

#if (true)
        if (token.IsCancellationRequested)
        {
            break;
        } 
#else
        token.ThrowIfCancellationRequested();
#endif

            Console.WriteLine($"Thread {managedThreadId}, i: {i} total= {total}");
            Thread.Sleep(100);
        }
        return total;
    }

    public static void Main()
    {
        var managedThreadId = Environment.CurrentManagedThreadId;
        
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Token.Register(() => Console.WriteLine($"Thread {managedThreadId}: Canceled!"));
        
        var task = new Task<int>(() => DoCompute(100, cancellationTokenSource.Token));
        task.Start();

        Thread.Sleep(2000);

        cancellationTokenSource.Cancel();

        try
        {
            task.Wait();
            var result = task.Result;
            Console.WriteLine($"Thread {managedThreadId} result: {result}");
        }
        catch (AggregateException exception)
        {
            Console.WriteLine($"Thread {managedThreadId} Exception: {exception.Message} ");
        }
    }
}