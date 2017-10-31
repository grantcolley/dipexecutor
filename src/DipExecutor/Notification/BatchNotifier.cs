//-----------------------------------------------------------------------
// https://github.com/aspnet/logging/blob/dev/src/Microsoft.Extensions.Logging.AzureAppServices/Internal/BatchingLoggerProvider.cs
// https://github.com/andrewlock/NetEscapades.Extensions.Logging
//-----------------------------------------------------------------------

using DipExecutor.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DipExecutor.Notification
{
    public class BatchNotifier : IBatchNotifier
    {
        private readonly List<StepNotification> currentBatch = new List<StepNotification>();
        private readonly TimeSpan interval;
        private readonly int? queueSize;
        private readonly int? batchSize;
        private readonly HttpClient httpClient;
        private BlockingCollection<StepNotification> notificationQueue;
        private Task outputTask;
        private CancellationTokenSource cancellationTokenSource;

        public BatchNotifier(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.GetHttpClient();

            // TODO: get this from config...
            interval = new TimeSpan(0, 0, 1);
            batchSize = null;
            queueSize = null;

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

            Start();
        }

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

        private void Start()
        {
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

        private async Task WriteNotificationAsync(IEnumerable<StepNotification> notifications, CancellationToken cancellationToken)
        {
            var logMessages = notifications.ToList();
            var jsonContent = JsonConvert.SerializeObject(logMessages);
            using (var response = await httpClient.PostAsync(notifications.First<StepNotification>().NotificationUrl, new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json")))
            {
                var content = await response.Content.ReadAsStringAsync();

                // fire and forget?
            }
        }

        private Task IntervalAsync(TimeSpan interval, CancellationToken cancellationToken)
        {
            return Task.Delay(interval, cancellationToken);
        }
    }
}
