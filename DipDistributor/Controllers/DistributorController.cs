//using DipRunner;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using System.IO;
//using System.Text;
//using System.Threading.Tasks;

//namespace DipDistributor.Controllers
//{
//    [Route("api/[controller]")]
//    public class DistributorController : Controller
//    {
//        // PUT api/distributor/run
//        [HttpPost]
//        [Route("Run")]
//        public async Task<IActionResult> Run([FromBody]Step step)
//        {
//            var distributor = new Distributor();
//            var response = await distributor.RunAsync(step);
//            return Content(JsonConvert.SerializeObject(response), "application/json", Encoding.UTF8);
//        }

//        // GET api/distributor/getdependency
//        [HttpGet]
//        [Route("GetDependency")]
//        public async Task<IActionResult> GetDependency(string file)
//        {
//            if (string.IsNullOrEmpty(file))
//            {
//                return null;
//            }

//            var stream = new FileStream(file, FileMode.Open, FileAccess.Read);
//            var response = File(stream, "application/octet-stream");
//            return response;
//        }

//        // GET api/distributor/isalive
//        [HttpGet]
//        [Route("IsAlive")]
//        public async Task<IActionResult> IsAlive()
//        {
//            return Content("alive");
//        }

//        // PUT api/distributor/log
//        [HttpPost]
//        [Route("Log")]
//        public async Task Log([FromBody]string value)
//        {
//            if (string.IsNullOrEmpty(value))
//            {
//                return;
//            }

//            await LogAsync(value);
//        }

//        private async Task LogAsync(string message)
//        {
//            // TODO: get from config...
//            string path = @"C:\GitHub\dipdistributor\DipDistributor.txt";

//            if (!System.IO.File.Exists(path))
//            {
//                using (StreamWriter sw = System.IO.File.CreateText(path))
//                {
//                    sw.WriteLine(message);
//                }

//                return;
//            }

//            using (StreamWriter sw = System.IO.File.AppendText(path))
//            {
//                sw.WriteLine(message);
//            }
//        }
//    }
//}
