using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using DipRunner;

namespace DipDistributor.Test
{
    [TestClass]
    public class DistributorTest
    {
        [TestMethod]
        public void CreateMessage_EmptyMessage()
        {
            // Arrange
            var distributor = new Distributor(null);

            // Act
            var result = distributor.CreateMessage("");

            // Assert
            Assert.IsTrue(result.Contains("   RunId: 0; Run Name: ; StepId: 0; Step Name: ; Step Status: Unknown"));
        }

        [TestMethod]
        public void CreateMessage_TestMessage()
        {
            // Arrange
            var distributor = new Distributor(null);

            // Act
            var result = distributor.CreateMessage("test");

            // Assert
            Assert.IsTrue(result.Contains("   RunId: 0; Run Name: ; StepId: 0; Step Name: ; Step Status: Unknown; Message: test"));
        }

        [TestMethod]
        public void CreateMessage_DefaultStepTestMessage()
        {
            // Arrange
            var distributor = new Distributor(null);
            var step = new Step();

            // Act
            var result = distributor.CreateMessage(step, "test");

            // Assert
            Assert.IsTrue(result.Contains("   RunId: 0; Run Name: ; StepId: 0; Step Name: ; Step Status: Unknown; Message: test"));
        }

        [TestMethod]
        public void CreateMessage_DefaultStepEmptyMessage()
        {
            // Arrange
            var distributor = new Distributor(null);
            var step = new Step();

            // Act
            var result = distributor.CreateMessage(step, "test");

            // Assert
            Assert.IsTrue(result.Contains("   RunId: 0; Run Name: ; StepId: 0; Step Name: ; Step Status: Unknown"));
        }

        [TestMethod]
        public void CreateMessage_StepTestMessage()
        {
            // Arrange
            var distributor = new Distributor(null);
            var step = new Step()
            {
                RunId = 1,
                RunName = "Test Run",
                StepId = 2,
                StepName = "Step Name",
                Status = StepStatus.InProgress
            };

            // Act
            var result = distributor.CreateMessage(step, "test");

            // Assert
            Assert.IsTrue(result.Contains("   RunId: 1; Run Name: Test Run; StepId: 2; Step Name: Step Name; Step Status: InProgress; Message: test"));
        }

        [TestMethod]
        public async Task LogAsync()
        {
            // Arrange
            var messageHandler = new TestMessageHandler<string>();
            var clientFactory = new DistributorTestHttpClientFactory<string>(messageHandler);
            var distributor = new Distributor(clientFactory);
            var step = new Step();
            step.Urls = new[] { "http://localhost:5000" };

            // Act
            await distributor.LogAsync(step, "test");

            // Assert
        }
    }
}
