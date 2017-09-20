using System.Net.Http;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DipDistributor.Test")]
namespace DipDistributor
{
    internal abstract class HttpClientFactory
    {
        internal abstract HttpClient GetHttpClient(HttpClientResponseContentType httpClientResponseType = HttpClientResponseContentType.StringContent);
    }
}