using System.IO;

namespace DipExecutor.Utilities
{
    internal static class Logger
    {
        private static object locker = new object();

        internal static void Log(string message)
        {
            // TODO: get from config...
            string path = @"C:\GitHub\dipexecutor\DipExecutor.txt";

            // TODO: clean this stuff up. Testing only.
            lock (locker)
            {
                if (!File.Exists(path))
                {
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(message);
                    }
                }

                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(message);
                }
            }
        }
    }
}
