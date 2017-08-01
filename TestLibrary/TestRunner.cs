using System.Threading.Tasks;
using DipDistribute;
using TestDependency;

namespace TestLibrary
{
    public class TestRunner : IRun
    {
        public async Task<Step> RunAsync(Step step)
        {
            return await Task.Run<Step>(() => 
            {
                var mydependency = new MyDependency();
                return mydependency.WriteMessage(step);
            });
        }
    }
}