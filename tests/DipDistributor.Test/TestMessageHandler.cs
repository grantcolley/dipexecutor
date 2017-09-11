using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DipDistributor.Test
{
    public class TestMessageHandler<T> : DelegatingHandler
    {
        Func<T, T> responseDelegate;

        public TestMessageHandler(Func<T, T> response)
        {
            responseDelegate = response;
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var content = await request.Content.ReadAsStringAsync();
            var deserializedContent = JsonConvert.DeserializeObject<T>(content);

            var responseContent = responseDelegate(deserializedContent);

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