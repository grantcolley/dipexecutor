using System.Collections.Generic;
using System.Linq;

namespace DipExecutor.Notification
{
    public class Subscribers
    {
        private readonly object lockObject;
        private readonly List<NotificationSubscriber> subscribers;

        public Subscribers()
        {
            lockObject = new object();
            subscribers = new List<NotificationSubscriber>();
        }

        public void Add(NotificationSubscriber subscriber)
        {
            if (subscriber != null)
            {
                lock (lockObject)
                {
                    if (!subscribers.Contains(subscriber))
                    {
                        subscribers.Add(subscriber);
                    }
                }
            }
        }

        public void Remove(NotificationSubscriber subscriber)
        {
            if (subscriber != null)
            {
                lock (lockObject)
                {
                    var remove = subscribers.FirstOrDefault(s => s.Url.ToLower().Equals(subscriber.Url.ToLower()));
                    if (subscribers.Contains(subscriber))
                    {
                        subscribers.Remove(subscriber);
                    }
                }
            }
        }

        public NotificationSubscriber[] Fetch(int runId)
        {
            lock(lockObject)
            {
                var result = subscribers.Where(s => s.RunId.Equals(runId)).ToArray();
                return result;
            }
        }
    }
}
