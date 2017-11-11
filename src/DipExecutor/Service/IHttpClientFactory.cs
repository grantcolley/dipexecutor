//-----------------------------------------------------------------------
// <copyright file="IHttpClientFactory.cs" company="Development In Progress Ltd">
//     Copyright © 2017. All rights reserved.
// </copyright>
// <author>Grant Colley</author>
//-----------------------------------------------------------------------

using System.Net.Http;

namespace DipExecutor.Service
{
    public interface IHttpClientFactory
    {
        HttpClient GetHttpClient(HttpClientResponseContentType httpClientResponseType = HttpClientResponseContentType.StringContent);
    }
}
