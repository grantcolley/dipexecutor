namespace DipExecutor.Notification
{
    public interface IBatchNotifierFactory<T>
    {
        IBatchNotifier<T> GetBatchNotifier(BatchNotifierType batchNotifierType);
    }
}