//-----------------------------------------------------------------------
// https://github.com/aspnet/logging/blob/dev/src/Microsoft.Extensions.Logging.AzureAppServices/Internal/BatchingLoggerProvider.cs
// https://github.com/andrewlock/NetEscapades.Extensions.Logging
//-----------------------------------------------------------------------

using DipExecutor.Service;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DipExecutor.Notification
{
    public abstract class BatchNotifier : IBatchNotifier
    {
        private readonly List<StepNotification> currentBatch = new List<StepNotification>();
        private BlockingCollection<StepNotification> notificationQueue;
        private Task outputTask;
        private CancellationTokenSource cancellationTokenSource;

        protected TimeSpan interval;
        protected int? queueSize;
        protected int? batchSize;

        public abstract Task WriteNotificationAsync(IEnumerable<StepNotification> notifications, CancellationToken cancellationToken);

        public void AddNotification(StepNotification notification)
        {
            if (!notificationQueue.IsAddingCompleted)
            {
                try
                {
                    notificationQueue.Add(notification, cancellationTokenSource.Token);
                }
                catch
                {
                    //cancellation token canceled or CompleteAdding called
                }
            }
        }

        internal void Stop()
        {
            cancellationTokenSource.Cancel();
            notificationQueue.CompleteAdding();

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

            notificationQueue = queueSize == null ?
                new BlockingCollection<StepNotification>(new ConcurrentQueue<StepNotification>()) :
                new BlockingCollection<StepNotification>(new ConcurrentQueue<StepNotification>(), queueSize.Value);

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

                while (limit > 0 && notificationQueue.TryTake(out var notification))
                {
                    currentBatch.Add(notification);
                    limit--;
                }

                if (currentBatch.Count > 0)
                {
                    try
                    {
                        await WriteNotificationAsync(currentBatch, cancellationTokenSource.Token);
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
