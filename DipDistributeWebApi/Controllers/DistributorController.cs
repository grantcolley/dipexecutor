using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace DipDistributeWebApi.Controllers
{
    [Route("api/[controller]")]
    public class DistributorController : Controller
    {
        [HttpGet]
        public HttpResponseMessage GetDependency(string path)
        {
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            return result;
        }

        [HttpPost]
        public void Log([FromBody]string value)
        {
        }
    }
}
