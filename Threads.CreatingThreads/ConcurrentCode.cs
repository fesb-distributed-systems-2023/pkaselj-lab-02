using System;
using System.Threading;

namespace Threads.CreatingThreads
{
    public class ConcurrentCode
    {
        private static int variable = 1;
        private readonly int field = 2;

        public static void Function()
        {
            var name = Thread.CurrentThread.Name;

            Console.WriteLine($"Function: Executing in thread: {name}");
            Console.WriteLine($"Function variable: {variable++}");
            Console.WriteLine("Function: Press any key to terminate.");
            Console.ReadKey();
        }

        public void Method(object? parameter)
        {
            var name = Thread.CurrentThread.Name;
            Console.WriteLine($"Method: Executing in thread: {name}");
            Console.WriteLine($"Method parameter: {parameter}");
            Console.WriteLine($"Method variable: {++variable}");
            Console.WriteLine($"Method field: {field}");
            Console.WriteLine("Method: Press any key to terminate.");
            Console.ReadKey();
        }

        public static void Method(string str, int num)
        {
            var name = Thread.CurrentThread.Name;
            Console.WriteLine($"Method: Executing in thread: {name}");
            Console.WriteLine($"Method str: {str}");
            Console.WriteLine($"Method num: {num}");
            Console.WriteLine("Method: Press any key to terminate.");
            Console.ReadKey();
        }
    }
}