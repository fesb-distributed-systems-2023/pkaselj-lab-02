using System;
using System.Threading;

namespace Threads.CreatingThreads
{
    public static class Program
    {

        private static readonly ThreadStart ts = new(ConcurrentCode.Function);
        private static readonly ConcurrentCode code = new();
        private static readonly ParameterizedThreadStart pts = new(code.Method);


        static void Main(string[] args)
        {
            var firstThread = new Thread(ts);
            firstThread.Name = "Zlatni konac litnje zore";
            //firstThread.IsBackground = true;
            firstThread.Start();

            var secondThread = new Thread(pts);
            secondThread.Name = "Der goldene Faden des sommerlichen Morgenrotes";
            secondThread.Priority = ThreadPriority.Highest;
            // secondThread.IsBackground = true;
            secondThread.Start(9);

            var i = 3;
            var thirdThread = new Thread(() => { ConcurrentCode.Method("Iva", i); });
            // thirdThread.IsBackground = true;
            i = 7;
            thirdThread.Start();
        }

    }
}
