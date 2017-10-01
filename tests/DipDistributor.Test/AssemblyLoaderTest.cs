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
        public async Task Stream()
        {
            // Arrange 
            var dependencies = new List<string>() { "DipRunner.dll", "TestLibrary.dll", "TestDependency" };

            var artefacts = Path.Combine(Directory.GetCurrentDirectory(), @"LoadAssembly\artefacts");

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

            var step = new Step();
            step.TargetAssembly = "TestLibrary.dll";
            step.TargetType = "TestLibrary.TestRunner";
            step.Payload = "1000|Hello";

            // Act 
            var assemblyLoader = new AssemblyLoader(artefacts, dependencies);
            var assembly = assemblyLoader.LoadFromAssemblyPath(Path.Combine(artefacts, step.TargetAssembly));
            var type = assembly.GetType(step.TargetType);
            dynamic obj = Activator.CreateInstance(type);
            var result = await obj.RunAsync(step);

            // Asssert
            Assert.AreEqual(result.Payload, "1000|Hello world!");
        }

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

            var step3 = new Step();
            step3.TargetAssembly = "TestLibrary.dll";
            step3.TargetType = "TestLibrary.TestRunner";
            step3.Payload = "10|Hello";

            var step4 = new Step();
            step4.TargetAssembly = "TestLibrary.dll";
            step4.TargetType = "TestLibrary.TestRunner";
            step4.Payload = "10|My";

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

            var assembly3 = assemblyLoader.LoadFromAssemblyPath(Path.Combine(artefacts, step3.TargetAssembly));
            var type3 = assembly3.GetType(step3.TargetType);
            dynamic obj3 = Activator.CreateInstance(type3);
            var result3 = await obj3.RunAsync(step3);

            var assemblyLoader4 = new AssemblyLoader(artefacts, dependencies);
            var assembly4 = assemblyLoader4.LoadFromAssemblyPath(Path.Combine(artefacts, step4.TargetAssembly));
            var type4 = assembly4.GetType(step4.TargetType);
            dynamic obj4 = Activator.CreateInstance(type4);
            var result4 = await obj4.RunAsync(step4);

            // Assert
            Assert.AreEqual(result.Payload, "1000|Hello world!");
            Assert.AreEqual(result2.Payload, "Hi there....");
            Assert.AreEqual(result3.Payload, "10|Hello world!");
            Assert.AreEqual(result4.Payload, "10|My world!");
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public async Task LoadAssembly_Dependencies_NotSpecified()
        {
            // Arrange
            var step = new Step();
            step.TargetAssembly = "TestLibrary.dll";
            step.TargetType = "TestLibrary.TestRunner";
            step.Payload = "10|The";

            var artefacts = Path.Combine(Directory.GetCurrentDirectory(), @"LoadAssembly\artefacts");
            
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

            // Act
            var assemblyLoader = new AssemblyLoader(artefacts, new List<string>());
            var assembly = assemblyLoader.LoadFromAssemblyPath(Path.Combine(artefacts, step.TargetAssembly));
            var type = assembly.GetType(step.TargetType);
            dynamic obj = Activator.CreateInstance(type);
            var result = await obj.RunAsync(step);

            // Assert
            Assert.AreEqual(result.Payload, "10|The world!");
        }
    }
}