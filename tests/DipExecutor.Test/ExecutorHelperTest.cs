using DipExecutor.Notification;
using DipExecutor.Utilities;
using DipRunner;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DipExecutor.Test
{
    [TestClass]
    public class ExecutorHelperTest
    {
        [TestMethod]
        public void CreateStepNotification()
        {
            // Arrange
            var step = TestHelper.GetStep();
            step.Status = StepStatus.InProgress;

            var message = "StackOverflow";

            // Act
            var stepNotification = ExecutorHelper.CreateStepNotification(step, NotificationLevel.Error, NotificationEvent.RunStepAsync, message);

            // Assert
            Assert.AreEqual(stepNotification.RunId, step.RunId);
            Assert.AreEqual(stepNotification.RunName, step.RunName);
            Assert.AreEqual(stepNotification.StepId, step.StepId);
            Assert.AreEqual(stepNotification.StepName, step.StepName);
            Assert.AreEqual(stepNotification.Status, step.Status);
            Assert.AreEqual(stepNotification.StepUrl, step.StepUrl);
            Assert.AreEqual(stepNotification.StepUrl, step.StepUrl);
            Assert.AreEqual(stepNotification.NotificationUrl, step.NotificationUrl);
            Assert.AreEqual(stepNotification.LoggingUrl, step.LogUrl);
            Assert.AreEqual(stepNotification.Message, message);
            Assert.AreEqual(stepNotification.NotificationLevel, NotificationLevel.Error);
            Assert.AreEqual(stepNotification.NotificationEventId, NotificationEvent.RunStepAsync);
            Assert.AreEqual(stepNotification.Machine, Environment.MachineName);
            Assert.AreEqual(stepNotification.Timestamp.ToString("yyyyddmm hhmm"), DateTimeOffset.Now.ToString("yyyyddmm hhmm"));
        }
    }
}