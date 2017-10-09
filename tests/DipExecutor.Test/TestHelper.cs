using DipRunner;
using System;
using System.Collections.Generic;
using System.IO;

namespace DipExecutor.Test
{
    public static class TestHelper
    {
        public static Step GetStep()
        {
            return new Step() { Urls = new[] { "http://localhost:5000/" } };
        }

        public static Step GetDistributedSteps(string runName, out IList<Step> steps)
        {
            var random = new Random();
            random.Next(4);

            steps = new List<Step>();

            var step1 = new Step();
            step1.RunId = 101;
            step1.RunName = runName;
            step1.StepId = 1;
            step1.StepName = "Step 1";
            step1.TargetAssembly = "TestLibrary.dll";
            step1.TargetType = "TestLibrary.TestRunner";
            step1.Payload = $"{random.Next(4)}|{step1.StepName} Hello";
            step1.Urls = new[] { "http://localhost:5000" };
            step1.Dependencies = new string[]
            {
                Path.Combine(@"..\..\..\artefacts","TestLibrary.dll"),
                Path.Combine(@"..\..\..\artefacts","TestDependency.dll")
            };

            var step21 = new Step();
            step21.RunId = 101;
            step21.RunName = runName;
            step21.StepId = 21;
            step21.StepName = "Step 2.1";
            step21.TargetAssembly = "TestLibrary.dll";
            step21.TargetType = "TestLibrary.TestRunner";
            step21.Payload = $"{random.Next(4)}|{step21.StepName} Hello";
            step21.Dependencies = new string[]
            {
                Path.Combine(@"..\..\..\artefacts","TestLibrary.dll"),
                Path.Combine(@"..\..\..\artefacts","TestDependency.dll")
            };

            var step22 = new Step();
            step22.RunId = 101;
            step22.RunName = runName;
            step22.StepId = 22;
            step22.StepName = "Step 2.2";
            step22.TargetAssembly = "TestLibrary.dll";
            step22.TargetType = "TestLibrary.TestRunner";
            step22.Payload = $"{random.Next(4)}|{step22.StepName} Hello";
            step22.Dependencies = new string[]
            {
                Path.Combine(@"..\..\..\artefacts","TestLibrary.dll"),
                Path.Combine(@"..\..\..\artefacts","TestDependency.dll")
            };

            var step23 = new Step();
            step23.RunId = 101;
            step23.RunName = runName;
            step23.StepId = 23;
            step23.StepName = "Step 2.3";
            step23.TargetAssembly = "TestLibrary.dll";
            step23.TargetType = "TestLibrary.TestRunner";
            step23.Payload = $"{random.Next(4)}|{step23.StepName} Hello";
            step23.Dependencies = new string[]
            {
                Path.Combine(@"..\..\..\artefacts","TestLibrary.dll"),
                Path.Combine(@"..\..\..\artefacts","TestDependency.dll")
            };

            var step3 = new Step();
            step3.RunId = 101;
            step3.RunName = runName;
            step3.StepId = 3;
            step3.StepName = "Step 3";
            step3.TargetAssembly = "TestLibrary.dll";
            step3.TargetType = "TestLibrary.TestRunner";
            step3.Payload = $"{random.Next(4)}|{step3.StepName} Hello";
            step3.Dependencies = new string[]
            {
                Path.Combine(@"..\..\..\artefacts","TestLibrary.dll"),
                Path.Combine(@"..\..\..\artefacts","TestDependency.dll")
            };

            var step31 = new Step();
            step31.RunId = 101;
            step31.RunName = runName;
            step31.StepId = 31;
            step31.StepName = "Step 3.1";
            step31.TargetAssembly = "TestLibrary.dll";
            step31.TargetType = "TestLibrary.TestRunner";
            step31.Payload = $"{random.Next(4)}|{step31.StepName} Hello";
            step31.Dependencies = new string[]
            {
                Path.Combine(@"..\..\..\artefacts","TestLibrary.dll"),
                Path.Combine(@"..\..\..\artefacts","TestDependency.dll")
            };

            var step4 = new Step();
            step4.RunId = 101;
            step4.RunName = runName;
            step4.StepId = 4;
            step4.StepName = "Step 4";
            step4.TargetAssembly = "TestLibrary.dll";
            step4.TargetType = "TestLibrary.TestRunner";
            step4.Payload = $"{random.Next(4)}|{step4.StepName} Hello";
            step4.Dependencies = new string[]
            {
                Path.Combine(@"..\..\..\artefacts","TestLibrary.dll"),
                Path.Combine(@"..\..\..\artefacts","TestDependency.dll")
            };

            step1.SubSteps = new[] { step21, step22, step23 };

            step3.SubSteps = new[] { step31 };

            step1.TransitionSteps = new[] { step3, step4 };

            return step1;
        }
    }
}
