using System.Net.Http;
using System.Net.Http.Headers;

namespace DipDistributor
{
    internal class DistributorHttpClientFactory : HttpClientFactory
    {
        private static HttpClient httpClient;
        private static object httpClientLock = new object();

        internal override HttpClient GetHttpClient()
        {
            if (httpClient == null)
            {
                lock (httpClientLock)
                {
                    if (httpClient == null)
                    {
                        httpClient = new HttpClient();
                        httpClient.MaxResponseContentBufferSize = 1000000;
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    }
                }
            }

            //https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
            //http://byterot.blogspot.co.uk/2016/07/singleton-httpclient-dns.html?m=1
            //var sp = ServicePointManager.FindServicePoint(new Uri("URI HERE....."));
            //sp.ConnectionLeaseTimeout = 60 * 1000;

            return httpClient;
        }
    }
}