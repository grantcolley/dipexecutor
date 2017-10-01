using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;
using System.Collections.Generic;
using System;

namespace DipDistributor
{
    internal class AssemblyLoader : AssemblyLoadContext
    {
        private string folderPath;
        private IList<string> dependencies;

        internal AssemblyLoader(string folderPath, IList<string> dependencies)
        {
            this.folderPath = folderPath;
            this.dependencies = dependencies;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            if ((dependencies?.Contains(assemblyName.Name) ?? false) == false)
            {
                return Assembly.Load(new AssemblyName(assemblyName.Name));
            }

            var deps = DependencyContext.Default;
            var res = deps.CompileLibraries.Where(d => d.Name.Contains(assemblyName.Name)).ToList();
            if (res.Count > 0)
            {
                return Assembly.Load(new AssemblyName(res.First().Name));
            }
            else
            {
                var apiApplicationFileInfo = new FileInfo($"{folderPath}{Path.DirectorySeparatorChar}{assemblyName.Name}.dll");
                if (File.Exists(apiApplicationFileInfo.FullName))
                {
                    var asl = new AssemblyLoader(apiApplicationFileInfo.DirectoryName, dependencies);
                    return asl.LoadFromAssemblyPath(apiApplicationFileInfo.FullName);
                }
            }

            return Assembly.Load(assemblyName);
        }
    }
}
