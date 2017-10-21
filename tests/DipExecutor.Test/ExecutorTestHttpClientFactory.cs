using DipExecutor.Service;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DipExecutor.Test
{
    public class ExecutorTestHttpClientFactory<T> : HttpClientFactory
    {
        private HttpClient httpClient;
        private object httpClientLock = new object();
        private TestHttpMessageHandler<T> messageHandler;

        public ExecutorTestHttpClientFactory(TestHttpMessageHandler<T> messageHandler)
        {
            this.messageHandler = messageHandler;
        }

        public override HttpClient GetHttpClient(HttpClientResponseContentType httpClientResponseContentType = HttpClientResponseContentType.StringContent)
        {
            if (httpClient == null)
            {
                lock (httpClientLock)
                {
                    if (httpClient == null)
                    {
                        httpClient = new HttpClient(messageHandler);
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    }
                }
            }
            
            return httpClient;
        }
    }
}
