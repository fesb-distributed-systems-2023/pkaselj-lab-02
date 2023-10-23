using System;
using System.Threading;

public static class Program
{
    private static readonly ThreadLocal<string> threadLocalData = new();
    private static readonly AsyncLocal<string> asyncLocalData = new();

    static void DoCompute(object? state)
    {
        var managedThreadId = Environment.CurrentManagedThreadId;
        
        Console.WriteLine($"Thread [{managedThreadId}], Task [{state}], BEGIN, Thread L.V. = '{threadLocalData.Value}', Async L.V. = '{asyncLocalData.Value}'");

        Thread.Sleep(TimeSpan.FromSeconds(4));

        Console.WriteLine($"Thread [{managedThreadId}], Task [{state}], END, Thread L.V. = '{threadLocalData.Value}', Async L.V. = '{asyncLocalData.Value}'");
    }

    public static void Main()
    {
        asyncLocalData.Value = "Ana";
        threadLocalData.Value = "Ana";
        
        for (int i = 0; i < 10; i++)
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            if (i == 5)
            {
                asyncLocalData.Value = "Marija";
                threadLocalData.Value = "Marija";
                // ExecutionContext.SuppressFlow();
            }
            ThreadPool.QueueUserWorkItem(DoCompute, i);
        }
        Console.WriteLine("Press any key");
        Console.ReadKey();
    }
}