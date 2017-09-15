using System.Net.Http;
using System.Net.Http.Headers;

namespace DipDistributor.Test
{
    internal class DistributorTestHttpClientFactory<T> : HttpClientFactory
    {
        private HttpClient httpClient;
        private object httpClientLock = new object();
        private TestMessageHandler<T> messageHandler;

        internal DistributorTestHttpClientFactory(TestMessageHandler<T> messageHandler)
        {
            this.messageHandler = messageHandler;
        }

        internal override HttpClient GetHttpClient()
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
