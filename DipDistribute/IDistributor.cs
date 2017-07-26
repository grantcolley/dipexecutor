namespace DipDistribute
{
    using System.Threading.Tasks;

    public interface IDistributor
    {
        Task<Step> RunAsync(Step step);
    }
}