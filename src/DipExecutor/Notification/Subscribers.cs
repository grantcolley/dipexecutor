using System.Collections.Generic;
using System.Linq;

namespace DipExecutor.Notification
{
    public class Subscribers
    {
        private readonly object lockObject;
        private readonly List<Subscriber> subscribers;

        public Subscribers()
        {
            lockObject = new object();
            subscribers = new List<Subscriber>();
        }

        public void Add(Subscriber subscriber)
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

        public void Remove(Subscriber subscriber)
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

        public IEnumerable<string> FetchUrls(int runId)
        {
            lock(lockObject)
            {
                var urls = from s in subscribers where s.RunId.Equals(runId) select s.Url;
                return urls;
            }
        }
    }
}