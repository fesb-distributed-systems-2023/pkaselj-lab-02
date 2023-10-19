using System;
using System.Threading;

public static class Program
{
    private static readonly Random random = new();
    private static readonly AsyncLocal<string> localData = new();

    static void DoCompute(object? state)
    {
        var managedThreadId = Environment.CurrentManagedThreadId;
        var name = localData.Value;
        
        Console.WriteLine($"Thread {managedThreadId}, State {state}, Name = \"{name}\"  ");
        Thread.Sleep(random.Next(1000, 1500));
    }

    public static void Main()
    {
        localData.Value = "Ana";
        
        for (int i = 0; i < 10; i++)
        {
            Thread.Sleep(random.Next(1000, 3000));
            if (i == 5)
            {
                ExecutionContext.SuppressFlow();
            }
            ThreadPool.QueueUserWorkItem(DoCompute, i);
        }
        Console.WriteLine("Press any key");
        Console.ReadKey();
    }
}