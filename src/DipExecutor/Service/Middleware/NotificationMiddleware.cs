﻿//-----------------------------------------------------------------------
// <copyright file="NotificationMiddleware.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using DipExecutor.Notification;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DipExecutor.Service.Middleware
{
    public class NotificationMiddleware
    {
        private readonly IBatchNotifier<IEnumerable<StepNotification>> batchNotifier;

        public NotificationMiddleware(RequestDelegate next, IBatchNotifierFactory<IEnumerable<StepNotification>> batchNotifierFactory)
        {
            this.batchNotifier = batchNotifierFactory.GetBatchNotifier(BatchNotifierType.ExecutorPublisher);
        }

        public async Task Invoke(HttpContext context)
        {
            string body;
            var stream = context.Request.Body;
            using (var reader = new StreamReader(stream))
            {
                body = await reader.ReadToEndAsync();
            }

            var stepNotifications = JsonConvert.DeserializeObject<List<StepNotification>>(body);
            
            batchNotifier.AddNotification(stepNotifications);
        }
    }
}