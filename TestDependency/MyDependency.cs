using DipDistribute;

namespace TestDependency
{
    public class MyDependency
    {
        public Step WriteMessage(Step step)
        {
            step.Payload += " world!";
            return step;
        }
    }
}
