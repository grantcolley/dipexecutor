using System.Net.Http;

namespace DipExecutor.Service
{
    public abstract class HttpClientFactory : IHttpClientFactory
    {
        public abstract HttpClient GetHttpClient(HttpClientResponseContentType httpClientResponseType = HttpClientResponseContentType.StringContent);
    }
}