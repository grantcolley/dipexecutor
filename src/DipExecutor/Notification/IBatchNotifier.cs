namespace DipExecutor.Notification
{
    public interface IBatchNotifier<T>
    {
        void AddNotification(T item);
    }
}