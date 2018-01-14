// https://github.com/aspnet/logging/blob/dev/src/Microsoft.Extensions.Logging.AzureAppServices/Internal/BatchingLoggerProvider.cs

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DipExecutor.Notification
{
    public abstract class BatchNotifier<T> : IBatchNotifier<T>
    {
        private readonly List<T> currentBatch = new List<T>();
        private BlockingCollection<T> notifyQueue;
        private Task outputTask;
        private CancellationTokenSource cancellationTokenSource;

        protected TimeSpan interval = new TimeSpan(0, 0, 0, 1);
        protected int? queueSize = default(int?);
        protected int? batchSize = default(int?);

        public abstract Task NotifyAsync(IEnumerable<T> items, CancellationToken cancellationToken);

        public void AddNotification(T item)
        {
            if (!notifyQueue.IsAddingCompleted)
            {
                try
                {
                    notifyQueue.Add(item, cancellationTokenSource.Token);
                }
                catch
                {
                    //cancellation token cancelled or CompleteAdding called
                }
            }
        }

        internal void Stop()
        {
            cancellationTokenSource.Cancel();
            notifyQueue.CompleteAdding();

            try
            {
                outputTask.Wait(interval);
            }
            catch (TaskCanceledException)
            {
            }
            catch (AggregateException ex) when (ex.InnerExceptions.Count == 1 && ex.InnerExceptions[0] is TaskCanceledException)
            {
            }
        }

        protected void Start()
        {
            if (batchSize.HasValue
                && batchSize <= 0)
            {
                throw new ArgumentOutOfRangeException("BatchSize", "BatchSize must be a positive number.");
            }

            if (queueSize.HasValue
                && queueSize <= 0)
            {
                throw new ArgumentOutOfRangeException("QueueSize", "QueueSize must be a positive number.");
            }

            if (interval <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("Interval", "Interval must be longer than zero.");
            }

            notifyQueue = queueSize == null ?
                new BlockingCollection<T>(new ConcurrentQueue<T>()) :
                new BlockingCollection<T>(new ConcurrentQueue<T>(), queueSize.Value);

            cancellationTokenSource = new CancellationTokenSource();

            outputTask = Task.Factory.StartNew<Task>(
                ProcessNotificationQueue,
                null,
                TaskCreationOptions.LongRunning);
        }

        private async Task ProcessNotificationQueue(object state)
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                var limit = batchSize ?? int.MaxValue;

                while (limit > 0 && notifyQueue.TryTake(out var notification))
                {
                    currentBatch.Add(notification);
                    limit--;
                }

                if (currentBatch.Count > 0)
                {
                    try
                    {
                        await NotifyAsync(currentBatch, cancellationTokenSource.Token);
                    }
                    catch
                    {
                        // ignored
                    }

                    currentBatch.Clear();
                }

                await IntervalAsync(interval, cancellationTokenSource.Token);
            }
        }

        private Task IntervalAsync(TimeSpan interval, CancellationToken cancellationToken)
        {
            return Task.Delay(interval, cancellationToken);
        }
    }
}
