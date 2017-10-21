using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("DipExecutor.Test")]
namespace DipExecutor.Service
{
    public enum HttpClientResponseContentType
    {
        StreamContent,
        StringContent
    }
}
