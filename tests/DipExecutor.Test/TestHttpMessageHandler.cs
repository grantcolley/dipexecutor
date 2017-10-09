using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DipExecutor.Test
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

            var requestContent = await request.Content.ReadAsStringAsync();

            HttpContent content = null;
            
            if (request.RequestUri.AbsolutePath.Equals("/log"))
            {
                if (responseDelegate != null)
                {
                    responseDelegate(responseContent, request.RequestUri.AbsolutePath);
                }

                content = new StringContent(JsonConvert.SerializeObject(responseContent), Encoding.UTF8, "application/json");
            }
            else if (request.RequestUri.AbsolutePath.Equals("/run"))
            {
                var deserializedContent = JsonConvert.DeserializeObject<T>(requestContent);
                if (responseDelegate != null)
                {
                    responseContent = responseDelegate(deserializedContent, request.RequestUri.AbsolutePath);
                }

                content = new StringContent(JsonConvert.SerializeObject(responseContent), Encoding.UTF8, "application/json");
            }
            else if (request.RequestUri.AbsolutePath.Equals("/getdependency"))
            {
                if (responseDelegate != null)
                {
                    responseDelegate(responseContent, request.RequestUri.AbsolutePath);
                }

                content = new StreamContent(new FileStream(requestContent, FileMode.Open, FileAccess.Read));
            }
            
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = content
            };

            var taskCompletionSource = new TaskCompletionSource<HttpResponseMessage>();
            taskCompletionSource.SetResult(response);
            return await taskCompletionSource.Task;
        }
    }
}