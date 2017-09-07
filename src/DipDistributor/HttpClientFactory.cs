using System.Net.Http;

namespace DipDistributor
{
    internal abstract class HttpClientFactory
    {
        internal abstract HttpClient GetHttpClient();
    }
}
