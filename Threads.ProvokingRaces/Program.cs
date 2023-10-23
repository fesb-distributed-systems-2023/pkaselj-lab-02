using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    private const int IncrementAmount = 10000;
    private static int sum = 0;
    private static readonly object incrementLock = new();
    private static readonly Stopwatch stopwatch = new();

    private static void IncrementSumSynchronously()
    {
        Console.WriteLine("Incrementing sum synchronously");

        stopwatch.Restart();
        for (var i = 0; i < IncrementAmount; i++)
        {
            sum++;
        }
        stopwatch.Stop();

        Console.WriteLine($"Duration: {stopwatch.Elapsed}, Sum value: {sum}");
    }

    private static void IncrementSumParallel_NoLock()
    {
        Console.WriteLine("Incrementing sum parallel");

        var allIncrementTasks = new List<Task>();

        stopwatch.Restart();
        for (var i = 0; i < IncrementAmount; i++)
        {
            var incrementTask = new Task(() => sum++);
            incrementTask.Start();
            allIncrementTasks.Add(incrementTask);
        }

        var allTasksFinishedTask = Task.WhenAll(allIncrementTasks);
        allTasksFinishedTask.Wait();
        stopwatch.Stop();

        Console.WriteLine($"Duration: {stopwatch.Elapsed}, Sum value: {sum}");
    }

    private static void IncrementSumParallel_WithLock()
    {
        Console.WriteLine("Incrementing sum parallel WITH a lock");

        var allIncrementTasks = new List<Task>();

        stopwatch.Restart();
        for (var i = 0; i < IncrementAmount; i++)
        {
            var incrementTask = new Task(() =>
            {
                lock (incrementLock)
                {
                    sum++;
                }
            });
            incrementTask.Start();
            allIncrementTasks.Add(incrementTask);
        }

        var allTasksFinishedTask = Task.WhenAll(allIncrementTasks);
        allTasksFinishedTask.Wait();
        stopwatch.Stop();

        Console.WriteLine($"Duration: {stopwatch.Elapsed}, Sum value: {sum}");
    }

    public static void Main()
    {
        IncrementSumSynchronously();
        sum = 0;
        IncrementSumParallel_NoLock();
        sum = 0;
        IncrementSumParallel_WithLock();
    }
}