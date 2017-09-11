using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DipDistributor.Test
{
    public class CustomMessageHandler : DelegatingHandler
    {
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var content = await request.Content.ReadAsStringAsync();
            content = JsonConvert.DeserializeObject<string>(content);
            content = content + " World";

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json")
            };

            var tcs = new TaskCompletionSource<HttpResponseMessage>();
            tcs.SetResult(response);
            return await tcs.Task;
        }
    }
}
