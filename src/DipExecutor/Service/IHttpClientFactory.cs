using System.Net.Http;

namespace DipExecutor.Service
{
    public interface IHttpClientFactory
    {
        HttpClient GetHttpClient(HttpClientResponseContentType httpClientResponseType = HttpClientResponseContentType.StringContent);
    }
}
