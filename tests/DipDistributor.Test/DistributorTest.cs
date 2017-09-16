using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using DipRunner;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
            var messageHandler = new TestMessageHandler<Step>();
            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);
            var step = new Step();
            step.Urls = new[] { "http://localhost:5000" };

            // Act
            await distributor.LogAsync(step, "test");

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetDependencyAssemblyNames_Dependencies_Null()
        {
            // Arrange
            var step = new Step();
            var distributor = new Distributor(null);

            // Act
            var dependencyList = distributor.GetDependencyAssemblyNames(step);

            // Assert
            Assert.AreEqual(dependencyList.Count, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetDependencyAssemblyNames_Dependencies_Empty()
        {
            // Arrange
            var step = new Step();
            var distributor = new Distributor(null);

            // Act
            var dependencyList = distributor.GetDependencyAssemblyNames(step);

            // Assert
            Assert.AreEqual(dependencyList.Count, 0);
        }

        [TestMethod]
        public void GetDependencyAssemblyNames_OneDependency()
        {
            // Arrange
            var distributor = new Distributor(null);
            var step = new Step();
            step.Dependencies = new[]
            {
                @"C:\GitHub\dipdistributor\TestLibrary\bin\Debug\netcoreapp2.0\TestDependency.dll"
            };

            // Act
            var dependencyList = distributor.GetDependencyAssemblyNames(step);

            // Assert
            Assert.AreEqual(dependencyList.Count, 1);
            Assert.AreEqual(dependencyList[0], "TestDependency");
        }

        [TestMethod]
        public void GetDependencyAssemblyNames_ManyDependencies()
        {
            // Arrange
            var distributor = new Distributor(null);
            var step = new Step();
            step.Dependencies = new[]
            {
                @"C:\GitHub\dipdistributor\TestLibrary\bin\Debug\netcoreapp2.0\DipRunner.dll",
                @"C:\GitHub\dipdistributor\TestLibrary\bin\Debug\netcoreapp2.0\TestDependency.dll",
                @"C:\GitHub\dipdistributor\TestLibrary\bin\Debug\netcoreapp2.0\TestLibrary.dll"
            };

            // Act
            var dependencyList = distributor.GetDependencyAssemblyNames(step);

            // Assert
            Assert.AreEqual(dependencyList.Count, 3);
            Assert.AreEqual(dependencyList[0], "DipRunner");
            Assert.AreEqual(dependencyList[1], "TestDependency");
            Assert.AreEqual(dependencyList[2], "TestLibrary");
        }

        [TestMethod]
        public void SetUrl_NoUrls_TwoSteps()
        {
            // Arrange
            var distributor = new Distributor(null);
            var steps = new Step[] { new Step(), new Step() };
            var urls = new string[] { };

            // Act
            var results = distributor.SetUrl(steps, urls);
            var resultsList = results.Cast<Step>().ToArray();

            // Assert
            Assert.AreEqual(resultsList.Length, 2);

            Assert.IsNull(resultsList[0].Urls);
            Assert.IsNull(resultsList[1].Urls);
        }

        [TestMethod]
        public void SetUrl_OneUrl_TwoSteps()
        {
            // Arrange
            var distributor = new Distributor(null);
            var steps = new Step[] { new Step(), new Step() };
            var urls = new string[] { "url1" };

            // Act
            var results = distributor.SetUrl(steps, urls);
            var resultsList = results.Cast<Step>().ToArray();

            // Assert
            Assert.AreEqual(resultsList.Length, 2);

            Assert.AreEqual(resultsList[0].Urls.Count(), 1);
            Assert.AreEqual(resultsList[0].StepUrl, $"{urls[0]}/run");

            Assert.AreEqual(resultsList[1].Urls.Count(), 1);
            Assert.AreEqual(resultsList[1].StepUrl, $"{urls[0]}/run");
        }

        [TestMethod]
        public void SetUrl_TwoUrls_TwoSteps()
        {
            // Arrange
            var distributor = new Distributor(null);
            var steps = new Step[] { new Step(), new Step() };
            var urls = new string[] { "url1", "url2" };

            // Act
            var results = distributor.SetUrl(steps, urls);
            var resultsList = results.Cast<Step>().ToArray();

            // Assert
            Assert.AreEqual(resultsList.Length, 2);

            Assert.AreEqual(resultsList[0].Urls.Count(), 2);
            Assert.AreEqual(resultsList[0].StepUrl, $"{urls[0]}/run");

            Assert.AreEqual(resultsList[1].Urls.Count(), 2);
            Assert.AreEqual(resultsList[1].StepUrl, $"{urls[1]}/run");
        }

        [TestMethod]
        public void SetUrl_ThreeUrls_TwoSteps()
        {
            // Arrange
            var distributor = new Distributor(null);
            var steps = new Step[] { new Step(), new Step() };
            var urls = new string[] { "url1", "url2", "url3" };

            // Act
            var results = distributor.SetUrl(steps, urls);
            var resultsList = results.Cast<Step>().ToArray();

            // Assert
            Assert.AreEqual(resultsList.Length, 2);

            Assert.AreEqual(resultsList[0].Urls.Count(), 3);
            Assert.AreEqual(resultsList[0].StepUrl, $"{urls[0]}/run");

            Assert.AreEqual(resultsList[1].Urls.Count(), 3);
            Assert.AreEqual(resultsList[1].StepUrl, $"{urls[1]}/run");
        }

        [TestMethod]
        public void SetUrl_TwoUrls_ThreeSteps()
        {
            // Arrange
            var distributor = new Distributor(null);
            var steps = new Step[] { new Step(), new Step(), new Step() };
            var urls = new string[] { "url1", "url2" };

            // Act
            var results = distributor.SetUrl(steps, urls);
            var resultsList = results.Cast<Step>().ToArray();

            // Assert
            Assert.AreEqual(resultsList.Length, 3);

            Assert.AreEqual(resultsList[0].Urls.Count(), 2);
            Assert.AreEqual(resultsList[0].StepUrl, $"{urls[0]}/run");

            Assert.AreEqual(resultsList[1].Urls.Count(), 2);
            Assert.AreEqual(resultsList[1].StepUrl, $"{urls[1]}/run");

            Assert.AreEqual(resultsList[2].Urls.Count(), 2);
            Assert.AreEqual(resultsList[2].StepUrl, $"{urls[0]}/run");
        }

        [TestMethod]
        public async Task DistributeStepAsync()
        {
            // Arrange
            var messageHandler = new TestMessageHandler<Step>((s, absolutePath) =>
            {
                if (absolutePath.Equals("/run"))
                {
                    s.Payload = "hello world";
                    s.Status = StepStatus.Complete;
                }

                return s;
            });

            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);
            var step = new Step();
            step.Urls = new[] { "http://localhost:5000/run" };

            // Act
            var result = await distributor.DistributeStepAsync(step);

            // Assert
            Assert.AreEqual(result.Payload, "hello world");
            Assert.AreEqual(result.Status, StepStatus.Complete);
        }

        [TestMethod]
        public async Task CompleteStepAsync()
        {
            // Arrange
            var messageHandler = new TestMessageHandler<Step>();
            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);
            var step = new Step();
            step.Urls = new[] { "http://localhost:5000" };

            // Act
            var result = await distributor.CompleteStepAsync(step);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(step.Status, StepStatus.Complete);
        }

        [TestMethod]
        public async Task RunTransitionStepsAsync_OneTransitionStepCompleted()
        {
            // Arrange
            var messageHandler = new TestMessageHandler<Step>((s, absolutePath) =>
            {
                if (absolutePath.Equals("/run"))
                {
                    s.Status = StepStatus.Complete;
                }

                return s;
            });

            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);
            
            var step = new Step();
            step.Urls = new[] { "http://localhost:5000" };
            step.TransitionSteps = new[] { new Step() };

            // Act
            var result = await distributor.RunTransitionStepsAsync(step);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task RunTransitionStepsAsync_TwoTransitionStepsCompleted()
        {
            // Arrange
            var messageHandler = new TestMessageHandler<Step>((s, absolutePath) =>
            {
                if (absolutePath.Equals("/run"))
                {
                    s.Status = StepStatus.Complete;
                }

                return s;
            });

            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);

            var step = new Step();
            step.Urls = new[] { "http://localhost:5000" };
            step.TransitionSteps = new[] { new Step(), new Step() };

            // Act
            var result = await distributor.RunTransitionStepsAsync(step);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task RunTransitionStepsAsync_TwoTransitionStepsOneNotCompleted()
        {
            // Arrange
            var messageHandler = new TestMessageHandler<Step>((s, absolutePath) =>
            {
                if (absolutePath.Equals("/run"))
                {
                    if (!s.StepName.Equals("transition1"))
                    {
                        s.Status = StepStatus.Complete;
                    }
                }

                return s;
            });

            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);

            var step = new Step();
            step.Urls = new[] { "http://localhost:5000" };
            step.TransitionSteps = new[] { new Step() { StepName = "transition1" }, new Step() };

            // Act
            var result = await distributor.RunTransitionStepsAsync(step);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task RunTransitionStepsAsync_OneTransitionStep_Exception()
        {
            // Arrange
            var messageHandler = new TestMessageHandler<Step>((s, absolutePath) =>
            {
                throw new DivideByZeroException();
            });

            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);

            var step = new Step();
            step.Urls = new[] { "http://localhost:5000" };
            step.TransitionSteps = new[] { new Step() };

            // Act
            var result = await distributor.RunTransitionStepsAsync(step);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task RunSubStepsAsync_TwoSubStepsCompleted()
        {
            // Arrange
            var messageHandler = new TestMessageHandler<Step>((s, absolutePath) =>
            {
                if (absolutePath.Equals("/run"))
                {
                    s.Status = StepStatus.Complete;
                }

                return s;
            });

            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);

            var step = new Step();
            step.Urls = new[] { "http://localhost:5000" };
            step.SubSteps = new[] { new Step(), new Step() };

            // Act
            var result = await distributor.RunSubStepsAsync(step);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task RunSubStepsAsync_TwoSubStepsOneNotCompleted()
        {
            // Arrange
            var messageHandler = new TestMessageHandler<Step>((s, absolutePath) =>
            {
                if (absolutePath.Equals("/run"))
                {
                    if (!s.StepName.Equals("subStep1"))
                    {
                        s.Status = StepStatus.Complete;
                    }
                }

                return s;
            });

            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);

            var step = new Step();
            step.Urls = new[] { "http://localhost:5000" };
            step.SubSteps = new[] { new Step() { StepName = "subStep1" }, new Step() };

            // Act
            var result = await distributor.RunSubStepsAsync(step);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task RunStepAsync_TargetAssemblyMissing()
        {
            // Arrange
            var messageHandler = new TestMessageHandler<Step>();
            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);

            var step = new Step();
            step.Urls = new[] { "http://localhost:5000" };

            // Act
            var result = await distributor.RunStepAsync(step);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task RunStepAsync_TargetTypeMissing()
        {
            // Arrange
            var messageHandler = new TestMessageHandler<Step>();
            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);

            var step = new Step();
            step.Urls = new[] { "http://localhost:5000" };
            step.TargetAssembly = "TestLibrary.dll";
            
            // Act
            var result = await distributor.RunStepAsync(step);

            // Assert
            Assert.IsTrue(result);
        }
    }
}