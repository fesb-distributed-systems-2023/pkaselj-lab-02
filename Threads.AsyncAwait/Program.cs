using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    private const int NumberOfIterations = 10;
    private const int ThreadPoolSize = 2;
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

    private static void DoWorkOnSeparateThreads()
    {
        var threads = new List<Thread>();
        for (var i = 0; i < NumberOfIterations; i++)
        {
            var thread = new Thread(Work);
            threads.Add(thread);
            thread.Start();
        }
        
        threads.ForEach(t => t.Join());
    }

    private static void DoWorkOnThreadPool()
    {
        int iWorkerThreadsOld;
        int iIOThreadsOld;
        ThreadPool.GetMaxThreads(out iWorkerThreadsOld, out iIOThreadsOld);
        ThreadPool.SetMaxThreads(ThreadPoolSize, iIOThreadsOld);

        using var finishedCounter = new CountdownEvent(NumberOfIterations);   
        for (var i = 0; i < NumberOfIterations; i++)
        {
            ThreadPool.QueueUserWorkItem(
                _ => {
                    Work();
                    finishedCounter.Signal();
                }
            );
        }

        finishedCounter.Wait();

        ThreadPool.SetMaxThreads(iWorkerThreadsOld, iIOThreadsOld);
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
        Console.WriteLine($"Started synchronous work.");
        stopwatch.Restart();
        DoWorkSynchronously();
        stopwatch.Stop();
        var syncCodeDuration = stopwatch.Elapsed;
        Console.WriteLine($"Synchronous code Duration {syncCodeDuration}");

        Console.WriteLine($"Started multi threaded work.");
        stopwatch.Restart();
        DoWorkOnSeparateThreads();
        stopwatch.Stop();
        var multiThreadCodeDuration = stopwatch.Elapsed;
        Console.WriteLine($"Multi threaded Duration {multiThreadCodeDuration}");

        Console.WriteLine($"Started thread pool work.");
        stopwatch.Restart();
        DoWorkOnThreadPool();
        stopwatch.Stop();
        var threadPoolDuration = stopwatch.Elapsed;
        Console.WriteLine($"Thread pool code Duration {threadPoolDuration}");

        Console.WriteLine($"Started parallel work.");
        stopwatch.Restart();
        await DoWorkParallel();
        stopwatch.Stop();
        var parallelCodeDuration = stopwatch.Elapsed;
        Console.WriteLine($"Parallel blocking code duration {parallelCodeDuration}");

        Console.WriteLine($"Started asynchronous parallel work.");
        stopwatch.Restart();
        await DoWorkParallelAsync();
        stopwatch.Stop();
        var asyncParallelCodeDuration = stopwatch.Elapsed;
        Console.WriteLine($"Asynchronous parallel code duration {asyncParallelCodeDuration}");
    }
}