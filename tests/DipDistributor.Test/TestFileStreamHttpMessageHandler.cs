using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DipDistributor.Test
{
    public class TestFileStreamHttpMessageHandler<T> : DelegatingHandler
    {
        Func<T, string, T> responseDelegate;

        public TestFileStreamHttpMessageHandler()
        {
        }

        public TestFileStreamHttpMessageHandler(Func<T, string, T> response)
        {
            responseDelegate = response;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            T responseContent = default(T);

            var content = await request.Content.ReadAsStringAsync();

            if (!request.RequestUri.AbsolutePath.Equals("/getdependency"))
            {
                var deserializedContent = JsonConvert.DeserializeObject<T>(content);
                if (responseDelegate != null)
                {
                    responseContent = responseDelegate(deserializedContent, request.RequestUri.AbsolutePath);
                }
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(new FileStream(content, FileMode.Open, FileAccess.Read))
            };
            
            var taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();
            taskCompletionSource.SetResult(response);
            return await taskCompletionSource.Task;
        }
    }
}
