using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DipExecutor.Test")]
namespace DipExecutor.Service
{
    internal enum HttpClientResponseContentType
    {
        StreamContent,
        StringContent
    }
}
