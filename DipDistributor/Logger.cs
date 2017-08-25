using System.IO;
using System.Threading.Tasks;

namespace DipDistributor
{
    public static class Logger
    {
        private static object locker = new object();

        public static async Task LogAsync(string message)
        {
            // TODO: get from config...
            string path = @"C:\GitHub\dipdistributor\DipDistributor.txt";

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
