using System.Net.Http;
using System.Net.Http.Headers;

namespace DipDistributor
{
    internal class DistributorHttpClientFactory : HttpClientFactory
    {
        internal override HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}