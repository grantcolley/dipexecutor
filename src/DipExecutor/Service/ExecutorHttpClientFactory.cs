//-----------------------------------------------------------------------
// <copyright file="ExecutorHttpClientFactory.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using System.Net.Http;
using System.Net.Http.Headers;

namespace DipExecutor.Service
{
    public class ExecutorHttpClientFactory : HttpClientFactory
    {
        private static HttpClient httpClientStringContentResponse;
        private static object httpClientStringContentResponseLock = new object();

        private static HttpClient httpClientStreamContentResponse;
        private static object httpClientStreamContentResponseLock = new object();

        public override HttpClient GetHttpClient(HttpClientResponseContentType httpClientResponseContentType = HttpClientResponseContentType.StringContent)
        {
            switch(httpClientResponseContentType)
            {
                case HttpClientResponseContentType.StringContent:
                    return GetHttpClientStringResponse();
                case HttpClientResponseContentType.StreamContent:
                    return GetHttpClientStreamResponse();
                default:
                    return GetHttpClientStringResponse();
            }
        }

        private HttpClient GetHttpClientStringResponse()
        {
            if (httpClientStringContentResponse == null)
            {
                lock (httpClientStringContentResponseLock)
                {
                    if (httpClientStringContentResponse == null)
                    {
                        httpClientStringContentResponse = new HttpClient();
                        httpClientStringContentResponse.DefaultRequestHeaders.Accept.Clear();
                        httpClientStringContentResponse.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    }
                }
            }

            //https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
            //http://byterot.blogspot.co.uk/2016/07/singleton-httpclient-dns.html?m=1
            //var sp = ServicePointManager.FindServicePoint(new Uri("URI HERE....."));
            //sp.ConnectionLeaseTimeout = 60 * 1000;

            return httpClientStringContentResponse;
        }

        private HttpClient GetHttpClientStreamResponse()
        {
            if (httpClientStreamContentResponse == null)
            {
                lock (httpClientStreamContentResponseLock)
                {
                    if (httpClientStreamContentResponse == null)
                    {
                        httpClientStreamContentResponse = new HttpClient();
                        httpClientStreamContentResponse.MaxResponseContentBufferSize = 1000000;
                        httpClientStreamContentResponse.DefaultRequestHeaders.Accept.Clear();
                        httpClientStreamContentResponse.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    }
                }
            }

            //https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
            //http://byterot.blogspot.co.uk/2016/07/singleton-httpclient-dns.html?m=1
            //var sp = ServicePointManager.FindServicePoint(new Uri("URI HERE....."));
            //sp.ConnectionLeaseTimeout = 60 * 1000;

            return httpClientStreamContentResponse;
        }
    }
}