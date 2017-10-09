using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DipExecutor.Test")]
namespace DipExecutor
{
    internal enum HttpClientResponseContentType
    {
        StreamContent,
        StringContent
    }
}
