//-----------------------------------------------------------------------
// <copyright file="Subscribers.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using System;
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

        public void Add(string connectionId, string runId)
        {

            if (string.IsNullOrWhiteSpace(connectionId)
                || string.IsNullOrWhiteSpace(runId))
            {
                return;
            }

            int id;
            if (Int32.TryParse(runId, out id))
            {
                lock (lockObject)
                {
                    var subscriber = subscribers.FirstOrDefault(s => s.ConnectionId.Equals(connectionId) && s.RunId.Equals(runId));
                    if (subscriber == null)
                    {
                        subscribers.Add(new Subscriber { ConnectionId = connectionId, RunId = id });
                    }
                }
            }
        }

        public void RemoveByRunId(string runId)
        {
            if (string.IsNullOrWhiteSpace(runId))
            {
                return;
            }

            int id;
            if (Int32.TryParse(runId, out id))
            {
                lock (lockObject)
                {
                    var subs = subscribers.Where(s => s.RunId.Equals(id)).ToList();
                    foreach (var subscriber in subs)
                    {
                        subscribers.Remove(subscriber);
                    }
                }
            }
        }

        public void RemoveByConnectionId(string connectionId)
        {
            if (string.IsNullOrWhiteSpace(connectionId))
            {
                return;
            }

            lock (lockObject)
            {
                var subs = subscribers.Where(s => s.ConnectionId.Equals(connectionId)).ToList();
                foreach (var subscriber in subs)
                {
                    subscribers.Remove(subscriber);
                }
            }
        }

        public IEnumerable<Subscriber> GetByRunId(string runId)
        {
            if(!string.IsNullOrWhiteSpace(runId))
            {
                int id;
                if (Int32.TryParse(runId, out id))
                {
                    lock (lockObject)
                    {
                        return subscribers.Where(s => s.RunId == id);
                    }
                }
            }

            return new List<Subscriber>();
        }
    }
}