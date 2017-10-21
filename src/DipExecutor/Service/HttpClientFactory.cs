using System.Net.Http;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DipExecutor.Test")]
namespace DipExecutor.Service
{
    public abstract class HttpClientFactory : IHttpClientFactory
    {
        public abstract HttpClient GetHttpClient(HttpClientResponseContentType httpClientResponseType = HttpClientResponseContentType.StringContent);
    }
}