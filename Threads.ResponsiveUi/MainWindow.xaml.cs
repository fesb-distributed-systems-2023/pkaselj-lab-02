using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace Threads.ResponsiveUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PrintThreadingInfoToConsole();
        }

        private static void LengthyOperation()
        {
            int tid = Environment.CurrentManagedThreadId;

            Debug.WriteLine($"Thread {tid} is working ... ");

            Thread.Sleep(7000);

            Debug.WriteLine($"Thread {tid} has finished. ");
        }

        private static void PrintThreadingInfoToConsole()
        {
            var nop = Environment.ProcessorCount;
            Debug.WriteLine($"Your system runs {nop} processors. ");

            var tid = Environment.CurrentManagedThreadId;
            Debug.WriteLine($"Thread {tid} is running ... ");
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            LengthyOperation();
        }

        private void ResponsiveButtonClick(object sender, RoutedEventArgs e)
        {
            new Thread(LengthyOperation).Start();
        }
    }
}
