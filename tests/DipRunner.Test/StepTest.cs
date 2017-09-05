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
        public void MandatoryField_Missing_StepUrl_LogUrl_Urls()
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
        public void MandatoryField_Missing_LogUrl_Urls()
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
        public void MandatoryField_Has_TargetAssembly_TargetType()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.Urls = new[] { "http:\\url1" };
            step.TargetAssembly = "Target Assembly";
            step.TargetType = "Target Type";

            // Act
            step.Validate();

            // Assert
        }

        [TestMethod]
        public void MandatoryField_WalkTree_No_SubSteps_TransitionSteps()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.Urls = new[] { "http:\\url1" };
            step.TargetAssembly = "Target Assembly";
            step.TargetType = "Target Type";

            // Act
            step.Validate(true);

            // Assert
        }
    }
}
