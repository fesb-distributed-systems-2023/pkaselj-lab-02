using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    private const int NumberOfIterations = 10;
    private static readonly TimeSpan WorkDuration = TimeSpan.FromSeconds(5);
    private static readonly Stopwatch stopwatch = new();
    private static void Work()
    {
        Task.Delay(WorkDuration).Wait();
    }
    private static async Task AsyncWork()
    {
        await Task.Delay(WorkDuration);
    }

    private static void DoWorkSynchronously()
    {
        for (var i = 0; i < NumberOfIterations; i++)
        {
            Work();
        }
    }

    private static async Task DoWorkParallel()
    {
        var tasks = new List<Task>();
        for (var i = 0; i < NumberOfIterations; i++)
        {
            var task = new Task(() => Work());
            task.Start();
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);
    }

    private static async Task DoWorkParallelAsync()
    {
        var tasks = new List<Task>();
        for (int i = 0; i < NumberOfIterations; i++)
        {
            var task = new Task(async () => await AsyncWork());
            task.Start();
            tasks.Add(task);
        }
        await Task.WhenAll(tasks);
    }

    public static async Task Main()
    {
        // 20 seconds is getting too long
        if (NumberOfIterations < 20)
        {
            Console.WriteLine($"Started synchronous work.");
            stopwatch.Restart();
            DoWorkSynchronously();
            stopwatch.Stop();
            var syncCodeDuration = stopwatch.Elapsed;
            Console.WriteLine($"Synchronous code Duration {syncCodeDuration}");
        }

        if (NumberOfIterations < 2000)
        {
            Console.WriteLine($"Started parallel work.");
            stopwatch.Restart();
            await DoWorkParallel();
            stopwatch.Stop();
            var parallelCodeDuration = stopwatch.Elapsed;
            Console.WriteLine($"Parallel blocking code duration {parallelCodeDuration}");
        }

        Console.WriteLine($"Started asynchronous parallel work.");
        stopwatch.Restart();
        await DoWorkParallelAsync();
        stopwatch.Stop();
        var asyncParallelCodeDuration = stopwatch.Elapsed;
        Console.WriteLine($"Asynchronous parallel code duration {asyncParallelCodeDuration}");
    }
}