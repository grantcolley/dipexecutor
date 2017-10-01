using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using DipRunner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace DipDistributor.Test
{
    [TestClass]
    public class DistributorTest
    {
        [TestMethod]
        public async Task DistributeStepAsync()
        {
            // Arrange
            var messageHandler = new TestHttpMessageHandler<Step>((s, absolutePath) =>
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
        public async Task LogAsync()
        {
            // Arrange
            var messageHandler = new TestHttpMessageHandler<Step>();
            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);
            var step = new Step();
            step.Urls = new[] { "http://localhost:5000" };

            // Act
            await distributor.LogAsync(step, "test");

            // Assert
        }

        [TestMethod]
        public async Task CompleteStepAsync()
        {
            // Arrange
            var messageHandler = new TestHttpMessageHandler<Step>();
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
            var messageHandler = new TestHttpMessageHandler<Step>((s, absolutePath) =>
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
            var messageHandler = new TestHttpMessageHandler<Step>((s, absolutePath) =>
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
            var messageHandler = new TestHttpMessageHandler<Step>((s, absolutePath) =>
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
            var messageHandler = new TestHttpMessageHandler<Step>((s, absolutePath) =>
            {
                if (absolutePath.Equals("/run"))
                {
                    throw new DivideByZeroException();
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
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task RunSubStepsAsync_TwoSubStepsCompleted()
        {
            // Arrange
            var messageHandler = new TestHttpMessageHandler<Step>((s, absolutePath) =>
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
            var messageHandler = new TestHttpMessageHandler<Step>((s, absolutePath) =>
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
        public void RunStepAsync_GetDependencyDirectory_TargetDownloadLocation()
        {
            // Arrange
            var messageHandler = new TestHttpMessageHandler<Step>();
            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);

            var step = new Step();
            step.RunName = "Test.RunName";
            step.StepName = "StepName";
            step.AssemblyPath = Path.Combine(Directory.GetCurrentDirectory(), "AssemblyPath");
            step.Urls = new[] { "http://localhost:5000" };

            // Act
            distributor.CreateAssemblyPath(step);

            // Assert
            Assert.AreEqual(step.AssemblyPath, Path.Combine(Directory.GetCurrentDirectory(), "AssemblyPath"));
            Assert.IsTrue(Directory.Exists(step.AssemblyPath));

            // Cleanup
            Directory.Delete(step.AssemblyPath);
        }

        [TestMethod]
        public void RunStepAsync_GetDependencyDirectory()
        {
            // Arrange
            var messageHandler = new TestHttpMessageHandler<Step>();
            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);

            var step = new Step();
            step.RunName = "Test.RunName";
            step.StepName = "StepName";
            step.Urls = new[] { "http://localhost:5000" };

            // Act
            distributor.CreateAssemblyPath(step);

            // Assert
            Assert.IsNotNull(step.AssemblyPath);
            Assert.IsTrue(Directory.Exists(step.AssemblyPath));

            // Cleanup
            Directory.Delete(step.AssemblyPath);

        }
        [TestMethod]
        public async Task RunStepAsync_TargetAssemblyMissing()
        {
            // Arrange
            var messageHandler = new TestHttpMessageHandler<Step>();
            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);

            var step = new Step();
            step.RunName = "Test";
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
            var messageHandler = new TestHttpMessageHandler<Step>();
            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);

            var step = new Step();
            step.RunName = "Test";
            step.Urls = new[] { "http://localhost:5000" };
            step.TargetAssembly = "TestLibrary.dll";

            // Act
            var result = await distributor.RunStepAsync(step);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task RunStepAsync()
        {
            // Arrange
            var messageHandler = new TestHttpMessageHandler<Step>();
            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);

            var step = new Step();
            step.RunName = "TestRunStepAsync";
            step.Urls = new[] { "http://localhost:5000" };
            step.TargetAssembly = "TestLibrary.dll";
            step.TargetType = "TestLibrary.TestRunner";
            step.Payload = "1000|Hello";
            step.Dependencies = new[]
            {
                Path.Combine(@"..\..\..\artefacts","DipRunner.dll"),
                Path.Combine(@"..\..\..\artefacts","TestDependency.dll"),
                Path.Combine(@"..\..\..\artefacts","TestLibrary.dll")
            };

            distributor.CreateAssemblyPath(step);

            if (File.Exists(Path.Combine(step.AssemblyPath, "TestLibrary.dll")))
            {
                File.Delete(Path.Combine(step.AssemblyPath, "TestLibrary.dll"));
            }

            if (File.Exists(Path.Combine(step.AssemblyPath, "TestDependency.dll")))
            {
                File.Delete(Path.Combine(step.AssemblyPath, "TestDependency.dll"));
            }

            File.Copy(@"..\..\..\artefacts\TestDependency.dll", Path.Combine(step.AssemblyPath, "TestDependency.dll"));
            File.Copy(@"..\..\..\artefacts\TestLibrary.dll", Path.Combine(step.AssemblyPath, "TestLibrary.dll"));

            // Act
            var result = await distributor.RunStepAsync(step);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(step.Payload, "1000|Hello world!");
        }

        [TestMethod]
        public async Task DownloadDependencyAsync()
        {
            // Arrange
            var messageHandler = new TestHttpMessageHandler<Step>();
            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);
            var httpClient = clientFactory.GetHttpClient();

            var step = new Step();
            step.RunName = "TestDownloadDependencyAsync";
            step.Urls = new[] { "http://localhost:5000" };
            step.DependencyUrl = "";
            step.Dependencies = new[]
            {
                Path.Combine(@"..\..\..\artefacts","TestLibrary.dll")
            };

            distributor.CreateAssemblyPath(step);

            if (File.Exists(Path.Combine(step.AssemblyPath, "TestLibrary.dll")))
            {
                File.Delete(Path.Combine(step.AssemblyPath, "TestLibrary.dll"));
            }

            // Act
            var result = await distributor.DownloadDependencyAsync(step, httpClient, step.Dependencies[0]);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(File.Exists(Path.Combine(step.AssemblyPath, "TestLibrary.dll")));
        }

        [TestMethod]
        public async Task DownloadDependenciesAsync()
        {
            // Arrange
            var messageHandler = new TestHttpMessageHandler<Step>();
            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);
            var httpClient = clientFactory.GetHttpClient();

            var step = new Step();
            step.RunName = "DownloadDependenciesAsync";
            step.Urls = new[] { "http://localhost:5000" };
            step.DependencyUrl = "";
            step.Dependencies = new[]
            {
                Path.Combine(@"..\..\..\artefacts","TestLibrary.dll"),
                Path.Combine(@"..\..\..\artefacts","TestDependency.dll")
            };

            distributor.CreateAssemblyPath(step);

            if (File.Exists(Path.Combine(step.AssemblyPath, "TestLibrary.dll")))
            {
                File.Delete(Path.Combine(step.AssemblyPath, "TestLibrary.dll"));
            }

            if (File.Exists(Path.Combine(step.AssemblyPath, "TestDependency.dll")))
            {
                File.Delete(Path.Combine(step.AssemblyPath, "TestDependency.dll"));
            }

            // Act
            var result = await distributor.DownloadDependenciesAsync(step);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(File.Exists(Path.Combine(step.AssemblyPath, "TestLibrary.dll")));
            Assert.IsTrue(File.Exists(Path.Combine(step.AssemblyPath, "TestDependency.dll")));
        }

        [TestMethod]
        public async Task InitialiseStepAsync()
        {
            // Arrange
            var messageHandler = new TestHttpMessageHandler<Step>();
            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);
            var httpClient = clientFactory.GetHttpClient();

            var step = new Step();
            step.RunName = "InitialiseStepAsync";
            step.Urls = new[] { "http://localhost:5000" };
            step.DependencyUrl = "";
            step.Dependencies = new[]
            {
                Path.Combine(@"..\..\..\artefacts","TestLibrary.dll"),
                Path.Combine(@"..\..\..\artefacts","TestDependency.dll")
            };

            distributor.CreateAssemblyPath(step);

            if (File.Exists(Path.Combine(step.AssemblyPath, "TestLibrary.dll")))
            {
                File.Delete(Path.Combine(step.AssemblyPath, "TestLibrary.dll"));
            }

            if (File.Exists(Path.Combine(step.AssemblyPath, "TestDependency.dll")))
            {
                File.Delete(Path.Combine(step.AssemblyPath, "TestDependency.dll"));
            }

            // Act
            var result = await distributor.InitialiseStepAsync(step);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(step.Status.Equals(StepStatus.Initialise));
            Assert.IsTrue(File.Exists(Path.Combine(step.AssemblyPath, "TestLibrary.dll")));
            Assert.IsTrue(File.Exists(Path.Combine(step.AssemblyPath, "TestDependency.dll")));
        }

        [TestMethod]
        public async Task ProcessStep()
        {
            // Arrange
            var messageHandler = new TestHttpMessageHandler<Step>((s, absolutePath) =>
            {
                if (absolutePath.Equals("/run"))
                {
                    s.Status = StepStatus.Complete;
                }

                return s;
            });

            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);
            var httpClient = clientFactory.GetHttpClient();

            IList<Step> steps;

            var step = TestHelper.GetDistributedSteps("ProcessStep", out steps);

            distributor.CreateAssemblyPath(step);

            if (File.Exists(Path.Combine(step.AssemblyPath, "TestLibrary.dll")))
            {
                File.Delete(Path.Combine(step.AssemblyPath, "TestLibrary.dll"));
            }

            if (File.Exists(Path.Combine(step.AssemblyPath, "TestDependency.dll")))
            {
                File.Delete(Path.Combine(step.AssemblyPath, "TestDependency.dll"));
            }

            // Act
            var result = await distributor.ProcessStepAsync(step);

            // Assert
            Assert.IsTrue(result.Status.Equals(StepStatus.Complete));
            Assert.IsTrue(File.Exists(Path.Combine(step.AssemblyPath, "TestLibrary.dll")));
            Assert.IsTrue(File.Exists(Path.Combine(step.AssemblyPath, "TestDependency.dll")));
        }

        [TestMethod]
        public async Task ProcessStep_InitialiseStepAsync_Unsuccessful()
        {
            // Arrange
            var messageHandler = new TestHttpMessageHandler<Step>((s, absolutePath) =>
            {
                if (absolutePath.Equals("/getdependency"))
                {
                    throw new Exception();
                }

                return s;
            });

            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);
            var httpClient = clientFactory.GetHttpClient();

            IList<Step> steps;

            var step = TestHelper.GetDistributedSteps("ProcessStep_InitialiseStepAsync_Unsuccessful", out steps);

            distributor.CreateAssemblyPath(step);

            if (File.Exists(Path.Combine(step.AssemblyPath, "TestLibrary.dll")))
            {
                File.Delete(Path.Combine(step.AssemblyPath, "TestLibrary.dll"));
            }

            if (File.Exists(Path.Combine(step.AssemblyPath, "TestDependency.dll")))
            {
                File.Delete(Path.Combine(step.AssemblyPath, "TestDependency.dll"));
            }

            // Act
            var result = await distributor.ProcessStepAsync(step);

            // Assert
            Assert.IsTrue(result.Status.Equals(StepStatus.Initialise));
            Assert.IsTrue(!File.Exists(Path.Combine(step.AssemblyPath, "TestLibrary.dll")));
            Assert.IsTrue(!File.Exists(Path.Combine(step.AssemblyPath, "TestDependency.dll")));
        }

        [TestMethod]
        public async Task ProcessStep_RunSubStepsAsync_Unsuccessful()
        {
            // Arrange
            var messageHandler = new TestHttpMessageHandler<Step>((s, absolutePath) =>
            {
                if (absolutePath.Equals("/run"))
                {
                    throw new Exception();
                }

                return s;
            });

            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);
            var httpClient = clientFactory.GetHttpClient();

            IList<Step> steps;

            var step = TestHelper.GetDistributedSteps("ProcessStep_RunSubStepsAsync_Unsuccessful", out steps);

            distributor.CreateAssemblyPath(step);

            if (File.Exists(Path.Combine(step.AssemblyPath, "TestLibrary.dll")))
            {
                File.Delete(Path.Combine(step.AssemblyPath, "TestLibrary.dll"));
            }

            if (File.Exists(Path.Combine(step.AssemblyPath, "TestDependency.dll")))
            {
                File.Delete(Path.Combine(step.AssemblyPath, "TestDependency.dll"));
            }

            // Act
            var result = await distributor.ProcessStepAsync(step);

            // Assert
            Assert.IsTrue(result.Status.Equals(StepStatus.InProgress));
            Assert.IsTrue(File.Exists(Path.Combine(step.AssemblyPath, "TestLibrary.dll")));
            Assert.IsTrue(File.Exists(Path.Combine(step.AssemblyPath, "TestDependency.dll")));
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task RunAsync_StepIsNull()
        {
            // Arrange
            var distributor = new Distributor(null);
            Step step = null;

            // Act
            var result = await distributor.RunAsync(step);

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public async Task RunAsync_Step_ValidationFalied()
        {
            // Arrange
            var distributor = new Distributor(null);
            var step = new Step();

            // Act
            var result = await distributor.RunAsync(step);

            // Assert
        }

        [TestMethod]
        public async Task RunAsync()
        {
            // Arrange
            var messageHandler = new TestHttpMessageHandler<Step>((s, absolutePath) =>
            {
                if (absolutePath.Equals("/run"))
                {
                    s.Status = StepStatus.Complete;
                }

                return s;
            });

            var clientFactory = new DistributorTestHttpClientFactory<Step>(messageHandler);
            var distributor = new Distributor(clientFactory);
            var httpClient = clientFactory.GetHttpClient();

            IList<Step> steps;

            var step = TestHelper.GetDistributedSteps("RunAsync", out steps);

            distributor.CreateAssemblyPath(step);

            if (File.Exists(Path.Combine(step.AssemblyPath, "TestLibrary.dll")))
            {
                File.Delete(Path.Combine(step.AssemblyPath, "TestLibrary.dll"));
            }

            if (File.Exists(Path.Combine(step.AssemblyPath, "TestDependency.dll")))
            {
                File.Delete(Path.Combine(step.AssemblyPath, "TestDependency.dll"));
            }

            // Act
            var result = await distributor.ProcessStepAsync(step);

            // Assert
            Assert.IsTrue(result.Status.Equals(StepStatus.Complete));
            Assert.IsTrue(File.Exists(Path.Combine(step.AssemblyPath, "TestLibrary.dll")));
            Assert.IsTrue(File.Exists(Path.Combine(step.AssemblyPath, "TestDependency.dll")));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void CreateMessage_NullMessage()
        {
            // Arrange
            var distributor = new Distributor(null);

            // Act
            var result = distributor.CreateMessage(null, "");

            // Assert
        }

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
                Path.Combine(@"..\..\..\artefacts","TestDependency.dll")
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
                Path.Combine(@"..\..\..\artefacts","DipRunner.dll"),
                Path.Combine(@"..\..\..\artefacts","TestDependency.dll"),
                Path.Combine(@"..\..\..\artefacts","TestLibrary.dll")
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
    }
}