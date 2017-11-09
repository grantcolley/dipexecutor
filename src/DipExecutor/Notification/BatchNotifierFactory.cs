using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DipExecutor.Test")]
namespace DipExecutor.Notification
{
    public abstract class BatchNotifierFactory<T> : IBatchNotifierFactory<T>
    {
        public abstract IBatchNotifier<T> GetBatchNotifier(BatchNotifierType batchNotifierType);
    }
}