using Microsoft.VisualStudio.TestTools.UnitTesting;
using DipRunner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DipDistributor.Test
{
    [TestClass]
    public class AssemblyLoaderTest
    {
        [TestMethod]
        public async Task LoadAssembly_LoadTwoAssemblies_SameNameType_DifferentFolder()
        {
            // Arrange
            var dependencies = new List<string>() { "DipRunner.dll", "TestLibrary.dll", "TestDependency" };
            var dependencies2 = new List<string>() { "DipRunner.dll", "TestLibrary.dll" };

            var step = new Step();
            step.TargetAssembly = "TestLibrary.dll";
            step.TargetType = "TestLibrary.TestRunner";
            step.Payload = "1000|Hello";

            var step2 = new Step();
            step2.TargetAssembly = "TestLibrary.dll";
            step2.TargetType = "TestLibrary.TestRunner";
            step2.Payload = "1000|Hello";

            var artefacts = Path.Combine(Directory.GetCurrentDirectory(), @"LoadAssembly\artefacts");
            var artefacts2 = Path.Combine(Directory.GetCurrentDirectory(), @"LoadAssembly\artefacts2");

            if (!Directory.Exists(artefacts))
            {
                Directory.CreateDirectory(artefacts);
            }

            if (File.Exists(Path.Combine(artefacts, "TestLibrary.dll")))
            {
                File.Delete(Path.Combine(artefacts, "TestLibrary.dll"));
            }

            if (File.Exists(Path.Combine(artefacts, "TestDependency.dll")))
            {
                File.Delete(Path.Combine(artefacts, "TestDependency.dll"));
            }

            File.Copy(@"..\..\..\artefacts\TestDependency.dll", Path.Combine(artefacts, "TestDependency.dll"));
            File.Copy(@"..\..\..\artefacts\TestLibrary.dll", Path.Combine(artefacts, "TestLibrary.dll"));

            if (!Directory.Exists(artefacts2))
            {
                Directory.CreateDirectory(artefacts2);
            }

            if (File.Exists(Path.Combine(artefacts2, "TestLibrary.dll")))
            {
                File.Delete(Path.Combine(artefacts2, "TestLibrary.dll"));
            }

            File.Copy(@"..\..\..\artefacts2\TestLibrary.dll", Path.Combine(artefacts2, "TestLibrary.dll"));

            // Act
            var assemblyLoader = new AssemblyLoader(artefacts, dependencies);
            var assembly = assemblyLoader.LoadFromAssemblyPath(Path.Combine(artefacts, step.TargetAssembly));
            var type = assembly.GetType(step.TargetType);
            dynamic obj = Activator.CreateInstance(type);
            var result = await obj.RunAsync(step);

            var assemblyLoader2 = new AssemblyLoader(artefacts2, dependencies2);
            var assembly2 = assemblyLoader2.LoadFromAssemblyPath(Path.Combine(artefacts2, step2.TargetAssembly));
            var type2 = assembly2.GetType(step2.TargetType);
            dynamic obj2 = Activator.CreateInstance(type2);
            var result2 = await obj2.RunAsync(step2);

            // Assert
            Assert.AreEqual(result.Payload, "1000|Hello world!");
            Assert.AreEqual(result2.Payload, "Hi there....");
        }
    }
}