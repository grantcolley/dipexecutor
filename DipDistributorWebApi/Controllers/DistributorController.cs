using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DipDistributor;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace DipDistributorWebApi.Controllers
{
    [Route("api/[controller]")]
    public class DistributorController : Controller
    {
        // GET api/distributor/getdependency
        [HttpGet]
        [Route("GetDependency")]
        public async Task<IActionResult> GetDependency(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                return null;
            }

            var stream = new FileStream(file, FileMode.Open, FileAccess.Read);
            var response = File(stream, "application/octet-stream");
            return response;
        }

        // PUT api/distributor/run
        [HttpPost]
        [Route("Run")]
        public async Task<IActionResult> Run([FromBody]Step step)
        {
            var distributor = new Distributor();
            var response = await distributor.RunAsync(step);
            return Content(JsonConvert.SerializeObject(response), "application/json", Encoding.UTF8);
        }

        // PUT api/distributor/log
        [HttpPost]
        [Route("Log")]
        public async void Log([FromBody]string value)
        {
            // TODO: write to log...
        }
    }
}
