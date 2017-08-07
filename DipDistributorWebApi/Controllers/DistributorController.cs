using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DipDistributor;

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
        public async void Run([FromBody]Step step)
        {
            var distributor = new Distributor();
            await distributor.RunAsync(step);
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
