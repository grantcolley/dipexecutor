using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

namespace DipDistributor.Test
{
    [TestClass]
    public class DistributorTest
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            // Arrange
            var messageHandler = new TestMessageHandler<string>(s => s += " World");

            var clientFactory = new DistributorTestHttpClientFactory<string>(messageHandler);
            var client = clientFactory.GetHttpClient();

            // Act
            var response = await client.PostAsync("http://localhost:5000/log", new StringContent(JsonConvert.SerializeObject("Hello"), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();
            var responseStep = JsonConvert.DeserializeObject<string>(content);

            // Assert
            Assert.AreEqual(responseStep, "Hello World");
        }
    }
}
