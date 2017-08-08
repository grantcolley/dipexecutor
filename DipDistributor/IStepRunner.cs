using System.Threading.Tasks;

namespace DipDistributor
{
    public interface IStepRunner
    {
        Task<Step> RunAsync(Step step);
    }
}