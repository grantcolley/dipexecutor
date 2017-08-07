using System.Threading.Tasks;

namespace DipDistributor
{
    public interface IRun
    {
        Task<Step> RunAsync(Step step);
    }
}