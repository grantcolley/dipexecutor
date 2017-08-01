using System.Threading.Tasks;

namespace DipDistribute
{
    public interface IRun
    {
        Task<Step> RunAsync(Step step);
    }
}