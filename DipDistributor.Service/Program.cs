using DipDistributor;

namespace DipDistributor.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new DistributorService();
            service.Run();
        }
    }
}