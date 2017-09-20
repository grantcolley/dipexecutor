using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DipDistributor.Test")]
namespace DipDistributor
{
    internal enum HttpClientResponseContentType
    {
        StreamContent,
        StringContent
    }
}
