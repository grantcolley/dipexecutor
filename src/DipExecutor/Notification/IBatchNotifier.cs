namespace DipExecutor.Notification
{
    public interface IBatchNotifier
    {
        void AddNotification(StepNotification notification);
    }
}