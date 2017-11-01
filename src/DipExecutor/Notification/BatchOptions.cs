//using System;

//namespace DipExecutor.Service.Notification
//{
//    public class BatchOptions
//    {
//        private int? batchSize = 32;
//        private int? backgroundQueueSize;
//        private TimeSpan interval = TimeSpan.FromSeconds(1);

//        /// <summary>
//        /// Gets or sets the period after which notifications will be flushed to the store.
//        /// </summary>
//        public TimeSpan Interval
//        {
//            get { return interval; }
//            set
//            {
//                if (value <= TimeSpan.Zero)
//                {
//                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(Interval)} must be positive.");
//                }

//                interval = value;
//            }
//        }

//        /// <summary>
//        /// Gets or sets the maximum size of the background notification message queue or null for no limit.
//        /// After maximum queue size is reached notifier event sink would start blocking.
//        /// </summary>
//        public int? BackgroundQueueSize
//        {
//            get { return backgroundQueueSize; }
//            set
//            {
//                if (value < 0)
//                {
//                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(BackgroundQueueSize)} must be non-negative.");
//                }

//                backgroundQueueSize = value;
//            }
//        }

//        /// <summary>
//        /// Gets or sets a maximum number of notifications to include in a single batch or null for no limit.
//        /// </summary>
//        public int? BatchSize
//        {
//            get { return batchSize; }
//            set
//            {
//                if (value <= 0)
//                {
//                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(BatchSize)} must be positive.");
//                }

//                batchSize = value;
//            }
//        }
//    }
//}
