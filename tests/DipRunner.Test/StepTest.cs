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
        public void MandatoryField_Missing_StepUrl()
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
        public void MandatoryField_Has_StepUrl()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.StepUrl = "http:\\stepurl";

            // Act
            step.Validate();

            // Assert
        }

        [TestMethod]
        public void MandatoryField_Has_Urls()
        {
            // Arrange
            var exceptionMessage = string.Empty;
            var step = new Step();
            step.RunName = "Run Name 1";
            step.StepName = "Step Name 1";
            step.Urls = new[] { "http:\\stepurl" };

            // Act
            step.Validate();

            // Assert
        }
    }
}
