using System.Threading.Tasks;

namespace DipRunner
{
    public interface IRunner
    {
        Task<Step> RunAsync(Step step);
    }
}