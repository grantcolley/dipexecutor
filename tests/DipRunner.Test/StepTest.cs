using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DipRunner.Test
{
    [TestClass]
    public class StepTest
    {
        [TestMethod]
        public void MandatoryField_Missing_RunName()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();

            try
            {
                // Act
                step.Validate();
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            // Assert
            Assert.IsTrue(exceptionMessage.Contains("Run name is missing"));
        }

        [TestMethod]
        public void MandatoryField_Missing_StepName()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";

            try
            {
                // Act
                step.Validate();
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            // Assert
            Assert.IsTrue(exceptionMessage.Contains("Step name is missing"));
        }

        [TestMethod]
        public void MandatoryField_Missing_StepUrl_NotificationUrl_Urls()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";

            try
            {
                // Act
                step.Validate();
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            // Assert
            Assert.IsTrue(exceptionMessage.Contains("Step url is missing"));
        }

        [TestMethod]
        public void MandatoryField_Missing_NotificationUrl_Urls()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.StepUrl = "http:\\stepurl";

            try
            {
                // Act
                step.Validate();
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            // Assert
            Assert.IsTrue(exceptionMessage.Contains("Notification url is missing"));
        }

        [TestMethod]
        public void MandatoryField_Missing_LogUrl_Urls()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.StepUrl = "http:\\stepurl";
            step.NotificationUrl = "http:\\notificationurl";

            try
            {
                // Act
                step.Validate();
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            // Assert
            Assert.IsTrue(exceptionMessage.Contains("Log url is missing"));
        }

        [TestMethod]
        public void MandatoryField_Missing_TargetAssembly_TargetType_SubSteps()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.StepUrl = "http:\\stepurl";
            step.NotificationUrl = "http:\\notifyurl";
            step.LogUrl = "http:\\logurl";

            try
            {
                // Act
                step.Validate();
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            // Assert
            Assert.IsTrue(exceptionMessage.Contains("If TargetType or TargetAssembly is missing then at least one sub step is required"));
        }

        [TestMethod]
        public void MandatoryField_Has_Urls()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.Urls = new[] { "http:\\url1" };

            try
            {
                // Act
                step.Validate();
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            // Assert
            Assert.IsTrue(exceptionMessage.Contains("If TargetType or TargetAssembly is missing then at least one sub step is required"));
        }

        [TestMethod]
        public void MandatoryField_Missing_TargetAssembly_SubSteps()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.Urls = new[] { "http:\\url1" };
            step.TargetAssembly = "Target Assembly";

            try
            {
                // Act
                step.Validate();
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            // Assert
            Assert.IsTrue(exceptionMessage.Contains("If TargetType or TargetAssembly is missing then at least one sub step is required"));
        }

        [TestMethod]
        public void MandatoryField_Missing_TargetType_SubSteps()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.Urls = new[] { "http:\\url1" };
            step.TargetType = "Target Type";

            try
            {
                // Act
                step.Validate();
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            // Assert
            Assert.IsTrue(exceptionMessage.Contains("If TargetType or TargetAssembly is missing then at least one sub step is required"));
        }

        [TestMethod]
        public void MandatoryField_Has_SubSteps()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.Urls = new[] { "http:\\url1" };
            step.SubSteps = new[] { new Step() };

            // Act
            step.Validate();

            // Assert
        }

        [TestMethod]
        public void MandatoryField_Has_TargetAssembly_TargetType_NoDependencies()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.Urls = new[] { "http:\\url1" };
            step.TargetAssembly = "Target Assembly";
            step.TargetType = "Target Type";

            try
            {
                // Act
                step.Validate();
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            // Assert
            Assert.IsTrue(exceptionMessage.Contains("If TargetType and TargetAssembly is missing then at least one dependency required i.e. the target assembly"));
        }

        [TestMethod]
        public void MandatoryField_Has_TargetAssembly_TargetType_HasDependency()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.Urls = new[] { "http:\\url1" };
            step.TargetAssembly = "Target Assembly";
            step.TargetType = "Target Type";
            step.Dependencies = new[] { "Dependency1" };

            // Act
            step.Validate();

            // Assert
        }

        [TestMethod]
        public void WalkTree_No_SubSteps_TransitionSteps()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.Urls = new[] { "http:\\url1" };
            step.TargetAssembly = "Target Assembly";
            step.TargetType = "Target Type";
            step.Dependencies = new[] { "Dependency1" };

            // Act
            step.Validate(true);

            // Assert
        }

        [TestMethod]
        public void WalkTree_Fail_SubSteps_Validation()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.Urls = new[] { "http:\\url1" };
            step.TargetAssembly = "Target Assembly";
            step.TargetType = "Target Type";
            step.Dependencies = new[] { "Dependency1" };

            var subStep = new Step() { RunId = 1 };

            step.SubSteps = new[] { subStep };

            try
            {
                // Act
                step.Validate(true);
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            // Assert
            Assert.IsTrue(exceptionMessage.Equals($"RunId: { subStep.RunId } - Run name is missing."));
        }

        [TestMethod]
        public void WalkTree_Pass_SubSteps_Validation()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.Urls = new[] { "http:\\url1" };
            step.TargetAssembly = "Target Assembly";
            step.TargetType = "Target Type";
            step.Dependencies = new[] { "Dependency1" };

            var subStep = new Step();
            subStep.RunName = "Run Name 1";
            subStep.StepName = "Sub Step Name 1";
            subStep.Urls = new[] { "http:\\url1" };
            subStep.TargetAssembly = "Target Assembly";
            subStep.TargetType = "Target Type";
            subStep.Dependencies = new[] { "Dependency1" };

            step.SubSteps = new[] { subStep };

            // Act
            step.Validate(true);

            // Assert
        }

        [TestMethod]
        public void WalkTree_Fail_Multiple_SubSteps_Validation()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.Urls = new[] { "http:\\url1" };
            step.TargetAssembly = "Target Assembly";
            step.TargetType = "Target Type";
            step.Dependencies = new[] { "Dependency1" };

            var subStep1 = new Step();
            subStep1.RunName = "Run Name 1";
            subStep1.StepName = "Sub Step Name 1";
            subStep1.Urls = new[] { "http:\\url1" };
            subStep1.TargetAssembly = "Target Assembly";
            subStep1.TargetType = "Target Type";
            subStep1.Dependencies = new[] { "Dependency1" };

            var subStep2 = new Step() { RunId = 2 };

            step.SubSteps = new[] { subStep1, subStep2 };

            try
            {
                // Act
                step.Validate(true);
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            // Assert
            Assert.IsTrue(exceptionMessage.Equals($"RunId: { subStep2.RunId } - Run name is missing."));
        }

        [TestMethod]
        public void WalkTree_Pass_Multiple_SubSteps_Validation()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.Urls = new[] { "http:\\url1" };
            step.TargetAssembly = "Target Assembly";
            step.TargetType = "Target Type";
            step.Dependencies = new[] { "Dependency1" };

            var subStep1 = new Step();
            subStep1.RunName = "Run Name 1";
            subStep1.StepName = "Sub Step Name 1";
            subStep1.Urls = new[] { "http:\\url1" };
            subStep1.TargetAssembly = "Target Assembly";
            subStep1.TargetType = "Target Type";
            subStep1.Dependencies = new[] { "Dependency1" };

            var subStep2 = new Step();
            subStep2.RunName = "Run Name 1";
            subStep2.StepName = "Sub Step Name 2";
            subStep2.Urls = new[] { "http:\\url1" };
            subStep2.TargetAssembly = "Target Assembly";
            subStep2.TargetType = "Target Type";
            subStep2.Dependencies = new[] { "Dependency1" };

            step.SubSteps = new[] { subStep1, subStep2 };

            // Act
            step.Validate(true);

            // Assert
        }

        [TestMethod]
        public void WalkTree_Fail_TransitionSteps_Validation()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.Urls = new[] { "http:\\url1" };
            step.TargetAssembly = "Target Assembly";
            step.TargetType = "Target Type";
            step.Dependencies = new[] { "Dependency1" };

            var transitionStep = new Step() { RunId = 1 };

            step.TransitionSteps = new[] { transitionStep };

            try
            {
                // Act
                step.Validate(true);
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            // Assert
            Assert.IsTrue(exceptionMessage.Equals($"RunId: { transitionStep.RunId } - Run name is missing."));
        }

        [TestMethod]
        public void WalkTree_Pass_TransitionSteps_Validation()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.Urls = new[] { "http:\\url1" };
            step.TargetAssembly = "Target Assembly";
            step.TargetType = "Target Type";
            step.Dependencies = new[] { "Dependency1" };

            var transitionStep = new Step();
            transitionStep.RunName = "Run Name 1";
            transitionStep.StepName = "Transition Step Name 1";
            transitionStep.Urls = new[] { "http:\\url1" };
            transitionStep.TargetAssembly = "Target Assembly";
            transitionStep.TargetType = "Target Type";
            transitionStep.Dependencies = new[] { "Dependency1" };

            step.TransitionSteps = new[] { transitionStep };

            // Act
            step.Validate(true);

            // Assert
        }

        [TestMethod]
        public void WalkTree_Fail_Multiple_TransitionSteps_Validation()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.Urls = new[] { "http:\\url1" };
            step.TargetAssembly = "Target Assembly";
            step.TargetType = "Target Type";
            step.Dependencies = new[] { "Dependency1" };

            var transitionStep1 = new Step();
            transitionStep1.RunName = "Run Name 1";
            transitionStep1.StepName = "Transition Step Name 1";
            transitionStep1.Urls = new[] { "http:\\url1" };
            transitionStep1.TargetAssembly = "Target Assembly";
            transitionStep1.TargetType = "Target Type";
            transitionStep1.Dependencies = new[] { "Dependency1" };

            var transitionStep2 = new Step() { RunId = 2 };

            step.TransitionSteps = new[] { transitionStep1, transitionStep2 };

            try
            {
                // Act
                step.Validate(true);
            }
            catch (Exception ex)
            {
                exceptionMessage = ex.Message;
            }

            // Assert
            Assert.IsTrue(exceptionMessage.Equals($"RunId: { transitionStep2.RunId } - Run name is missing."));
        }

        [TestMethod]
        public void WalkTree_Pass_Multiple_TransitionSteps_Validation()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.Urls = new[] { "http:\\url1" };
            step.TargetAssembly = "Target Assembly";
            step.TargetType = "Target Type";
            step.Dependencies = new[] { "Dependency1" };

            var transitionStep1 = new Step();
            transitionStep1.RunName = "Run Name 1";
            transitionStep1.StepName = "Transition Step Name 1";
            transitionStep1.Urls = new[] { "http:\\url1" };
            transitionStep1.TargetAssembly = "Target Assembly";
            transitionStep1.TargetType = "Target Type";
            transitionStep1.Dependencies = new[] { "Dependency1" };

            var transitionStep2 = new Step();
            transitionStep2.RunName = "Run Name 1";
            transitionStep2.StepName = "Transition Step Name 2";
            transitionStep2.Urls = new[] { "http:\\url1" };
            transitionStep2.TargetAssembly = "Target Assembly";
            transitionStep2.TargetType = "Target Type";
            transitionStep2.Dependencies = new[] { "Dependency1" };

            step.TransitionSteps = new[] { transitionStep1, transitionStep2 };

            // Act
            step.Validate(true);

            // Assert
        }

        [TestMethod]
        public void GetUrlAction_Url_Empty()
        {
            // Arrange
            var step = new Step();

            // Act
            var url = step.GetUrlAction(string.Empty, "test");

            // Assert
            Assert.AreEqual(url, "");
        }

        [TestMethod]
        public void GetUrlAction_Url_Action()
        {
            // Arrange
            var step = new Step();

            // Act
            var url = step.GetUrlAction(@"http:\\url1", "test");

            // Assert
            Assert.AreEqual(url, @"http:\\url1/test");
        }

        [TestMethod]
        public void GetUrlAction_Url_TrailingForwardslash()
        {
            // Arrange
            var step = new Step();

            // Act
            var url = step.GetUrlAction(@"http:\\url1/", "test");

            // Assert
            Assert.AreEqual(url, @"http:\\url1/test");
        }

        [TestMethod]
        public void GetUrlAction_StepUrl()
        {
            // Arrange
            var step = new Step();
            step.Urls = new[] { "http:\\url1" };
            // Act
            var url = step.StepUrl;

            // Assert
            Assert.AreEqual(url, "http:\\url1/run");
        }

        [TestMethod]
        public void GetUrlAction_NotifyUrl()
        {
            // Arrange
            var step = new Step();
            step.Urls = new[] { "http:\\url1" };
            // Act
            var url = step.NotificationUrl;

            // Assert
            Assert.AreEqual(url, "http:\\url1/notify");
        }

        [TestMethod]
        public void GetUrlAction_LogUrl()
        {
            // Arrange
            var step = new Step();
            step.Urls = new[] { "http:\\url1" };
            // Act
            var url = step.LogUrl;

            // Assert
            Assert.AreEqual(url, "http:\\url1/log");
        }

        [TestMethod]
        public void GetUrlAction_DependencyUrl()
        {
            // Arrange
            var step = new Step();
            step.Urls = new[] { "http:\\url1" };
            // Act
            var url = step.DependencyUrl;

            // Assert
            Assert.AreEqual(url, "http:\\url1/getdependency");
        }
    }
}
