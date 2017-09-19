using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DipDistributor.Test
{
    public class TestHttpMessageHandler<T> : DelegatingHandler
    {
        Func<T, string, T> responseDelegate;

        public TestHttpMessageHandler()
        {
        }

        public TestHttpMessageHandler(Func<T, string, T> response)
        {
            responseDelegate = response;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            T responseContent = default(T);

            var content = await request.Content.ReadAsStringAsync();

            if(!request.RequestUri.AbsolutePath.Equals("/log"))
            {
                var deserializedContent = JsonConvert.DeserializeObject<T>(content);
                if (responseDelegate != null)
                {
                    responseContent = responseDelegate(deserializedContent, request.RequestUri.AbsolutePath);
                }
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(responseContent), Encoding.UTF8, "application/json")
            };

            var taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();
            taskCompletionSource.SetResult(response);
            return await taskCompletionSource.Task;
        }
    }
}