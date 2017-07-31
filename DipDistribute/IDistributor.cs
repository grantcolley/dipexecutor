using System.Threading.Tasks;

namespace DipDistribute
{
    public interface IDistributor
    {
        Task<Step> RunAsync(Step step);
    }
}