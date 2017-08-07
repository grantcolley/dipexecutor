using System.Threading.Tasks;

namespace DipDistributor
{
    public interface IDistributor
    {
        Task<Step> RunAsync(Step step);
    }
}