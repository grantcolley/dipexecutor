using System.Net.Http;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DipExecutor.Test")]
namespace DipExecutor.Service
{
    internal abstract class HttpClientFactory
    {
        internal abstract HttpClient GetHttpClient(HttpClientResponseContentType httpClientResponseType = HttpClientResponseContentType.StringContent);
    }
}