using System.Collections;
using UnityEngine;

namespace Scaffold.Launcher.PackageHandler
{
    public class PackageValidator
    {
        public PackageValidator()
        {

        }

        private ScaffoldManifest module;

        public bool ValidateDependencies()
        {
            //get all modules on the project
            //get dependency of each package
            //check if all dependencies are there
            //request to install missing dependencies
            return true;
        }
        //public List<PackagePath> GetPackageDependencies(PackagePath package)
        //{
        //    if (!Modules.Contains(package))
        //    {
        //        return null;
        //    }

        //    List<PackagePath> dependencies = new List<PackagePath>();
        //    foreach (string packageName in package.dependencies)
        //    {
        //        PackagePath dependency = GetPackage(packageName);
        //        if (dependency == null)
        //        {
        //            continue;
        //        }

        //        if (dependencies.Contains(dependency))
        //        {
        //            continue;
        //        }

        //        dependencies.Add(dependency);
        //    }

        //    return dependencies;
        //}
    }
}